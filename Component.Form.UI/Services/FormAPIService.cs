using System;
using Component.Form.Model;
using Newtonsoft.Json;
using Component.Form.UI.Services.Model;

namespace Component.Form.UI.Services;

public class FormAPIService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private const string FormApiUrl = "api/getForm/";

    public FormAPIService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public async Task<FormModel> GetFormAsync(string formId)
    {
        try
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["FormAPI:BaseAddress"]);
            var response = await client.GetAsync($"{FormApiUrl}{formId}");
            
            response.EnsureSuccessStatusCode();
            
            var responseString = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(responseString))
            {
                throw new ApplicationException("The response content is empty.");
            }

            var formModel = JsonConvert.DeserializeObject<GetFormResponseModel>(responseString);
            if (formModel == null)
            {
                throw new ApplicationException("Failed to deserialize the form data.");
            }

            return formModel.Form;
        }
        catch (Exception ex)
        {
            // Handle exception (log it, rethrow it, or return a default value)
            throw new ApplicationException("An error occurred while getting form data.", ex);
        }
    }
}