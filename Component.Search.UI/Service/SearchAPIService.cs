using Component.Search.UI.Service.Model;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;

namespace Component.Search.UI.Service;

public class SearchAPIService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<SearchAPIService> _logger;
    private readonly AsyncRetryPolicy<HttpResponseMessage> _retryPolicy;

    public SearchAPIService(HttpClient httpClient, ILogger<SearchAPIService> logger, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _logger = logger;

        _httpClient.BaseAddress = new Uri(configuration["SearchAPI:BaseAddress"]);

        _retryPolicy = Policy
            .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
            .Or<HttpRequestException>()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                (outcome, timespan, retryAttempt, context) =>
                {
                    _logger.LogWarning("Delaying for {delay}ms, then making retry {retry}.", timespan.TotalMilliseconds, retryAttempt);
                });
    }

    public async Task<GetDataResponseModel> SearchAsync(GetDataRequestModel request)
    {
        try
        {
            var response = await _retryPolicy.ExecuteAsync(() => _httpClient.PostAsJsonAsync("api/search", request));
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(responseString))
            {
                throw new ApplicationException("The response content is empty.");
            }
            return JsonConvert.DeserializeObject<GetDataResponseModel>(responseString);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Request error: {Message}", ex.Message);
            throw new Exception("An error occurred while searching.", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error: {Message}", ex.Message);
            throw;
        }
    }

    public async Task<GetDataItemResponseModel> GetDetailAsync(GetDataItemRequestModel request)
    {
        try
        {
            var response = await _retryPolicy.ExecuteAsync(() => _httpClient.GetAsync($"api/detail/{request.ItemTypeId}/{request.ItemId}"));
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(responseString))
            {
                throw new ApplicationException("The response content is empty.");
            }
            return JsonConvert.DeserializeObject<GetDataItemResponseModel>(responseString);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Request error: {Message}", ex.Message);
            throw new Exception("An error occurred while getting detail.", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error: {Message}", ex.Message);
            throw;
        }
    }

    public async Task<GetSearchIndexResponseModel> GetSearchIndexTypeAsync()
    {
        try
        {
            var response = await _retryPolicy.ExecuteAsync(() => _httpClient.GetAsync("api/searchIndexType"));
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(responseString))
            {
                throw new ApplicationException("The response content is empty.");
            }
            return JsonConvert.DeserializeObject<GetSearchIndexResponseModel>(responseString);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Request error: {Message}", ex.Message);
            throw new Exception("An error occurred while fetching search index type.", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error: {Message}", ex.Message);
            throw;
        }
    }
}
