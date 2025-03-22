using Component.Form.Model;
using Component.Form.UI.Services.Model;
using Component.Form.Application.Helpers;
using Polly;
using Polly.Retry;
using Newtonsoft.Json;

namespace Component.Form.UI.Services;

public class FormAPIService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly SafeJsonHelper _safeJsonHelper;
    private const string GetFormApiUrl = "api/getForm/";
    private const string GetFormDataApiUrl = "api/getData/";
    private const string GetFormDataForPageApiUrl = "api/getDataForPage/";
    private const string ProcessFormApiUrl = "api/processForm/";
    private const string UpdateFormApiUrl = "api/updateForm/";
    private const string ProcessChangeApiUrl = "api/processChange/";
    private const string RemoveRepeatingSectionApiUrl = "api/removeRepeatingSection";
    private const string AddRepeatingSectionApiUrl = "api/addRepeatingSection";
    private readonly AsyncRetryPolicy _retryPolicy;

    public FormAPIService(IHttpClientFactory httpClientFactory, IConfiguration configuration, SafeJsonHelper safeJsonHelper)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _safeJsonHelper = safeJsonHelper;

        _retryPolicy = Policy
            .Handle<HttpRequestException>()
            .Or<TaskCanceledException>()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }

    public async Task<FormModel> GetFormAsync(string formId)
    {
        try
        {
            var formModel = await GetApiResponseAsync<GetFormResponseModel>($"{GetFormApiUrl}{formId}");
            return formModel.Form;
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"An error occurred while getting form for {formId}", ex);
        }
    }

    // TODO: refactor to carry form ID as well - deals with sharing forms on one instance..
    public async Task<GetDataResponseModel> GetFormDataAsync(string applicantId)
    {
        try
        {
            var dataModel = await GetApiResponseAsync<GetDataResponseModel>($"{GetFormDataApiUrl}{applicantId}");
            return dataModel;
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"An error occurred while getting form data for {applicantId}", ex);
        }
    }

    public async Task<GetDataForPageResponseModel> GetFormDataForPageAsync(string formId, string pageId, string applicantId, string data)
    {
        try
        {
            return await GetApiResponseAsync<GetDataForPageResponseModel>($"{GetFormDataForPageApiUrl}{formId}/{pageId}/{applicantId}/{data}");
        }
        catch (Exception ex)
        {
            throw new ApplicationException("An error occurred while getting form data for {formId} - {pageId} for {applicantId}", ex);
        }
    }

    public async Task<ProcessFormResponseModel> ProcessFormAsync(
        string formId, 
        string applicantId, 
        string pageId,
        Dictionary<string, string> formData)
    {
        try
        {
            var postModel = new ProcessFormRequestModel
            {
                FormId = formId,
                ApplicantId = applicantId,
                PageId = pageId,
                Form = formData
            };

            return await PostApiResponseAsync<ProcessFormResponseModel>(ProcessFormApiUrl, postModel);
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"An error occurred while processing form data on {formId} - {pageId} for applicant {applicantId}", ex);
        }
    }

        public async Task<ProcessFormResponseModel> ProcessChangeAsync(
        string formId, 
        string applicantId, 
        string pageId,
        Dictionary<string, string> formData)
    {
        try
        {
            var postModel = new ProcessFormRequestModel
            {
                FormId = formId,
                ApplicantId = applicantId,
                PageId = pageId,
                Form = formData
            };

            return await PostApiResponseAsync<ProcessFormResponseModel>(ProcessChangeApiUrl, postModel);
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"An error occurred while processing a change for {formId} - {pageId} for applicant {applicantId}", ex);
        }
    }

    public async Task<UpdateFormResponseModel> UpdateFormAsync(FormModel formModel)
    {
        try
        {
            var updateFormRequest = new UpdateFormRequestModel { Form = formModel };
            return await PostApiResponseAsync<UpdateFormResponseModel>(UpdateFormApiUrl, updateFormRequest);
        }
        catch (Exception ex)
        {
            throw new ApplicationException("An error occurred while updating form data.", ex);
        }
    }

    public async Task SubmitFormDataAsync(FormModel formModel, FormData data)
    {
        try
        {
            var client = _httpClientFactory.CreateClient();            
            var submissionEndpoint = formModel.Submission.Endpoint;
            if (string.IsNullOrWhiteSpace(submissionEndpoint))
            {
                throw new ApplicationException("The submission endpoint is not configured.");
            }
            var response = await client.PostAsJsonAsync(submissionEndpoint, data.Data);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            throw new ApplicationException("An error occurred while submitting form data.", ex);
        }
    }

    public async Task<RemoveRepeatingSectionResponseModel> RemoveRepeatingSection(string formId, string pageId, string applicantId, int index) 
    {
        try 
        {
            var removeRepeatingSectionRequest = new RemoveRepeatingSectionRequestModel 
            { 
                FormId = formId, 
                PageId = pageId, 
                ApplicantId = applicantId, 
                Index = index 
            };
            return await PostApiResponseAsync<RemoveRepeatingSectionResponseModel>(
                $"{RemoveRepeatingSectionApiUrl}", removeRepeatingSectionRequest);
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"An error occurred while removing {index} section on {formId} - {pageId} for applicant {applicantId}", ex);
        } 
    }

    public async Task<AddRepeatingSectionResponseModel> AddRepeatingSection(string formId, string pageId, string applicantId) 
    {
        try 
        {
            var addRepeatingSectionRequest = new AddRepeatingSectionRequestModel 
            { 
                FormId = formId, 
                PageId = pageId, 
                ApplicantId = applicantId  
            };
            return await PostApiResponseAsync<AddRepeatingSectionResponseModel>(
                AddRepeatingSectionApiUrl, addRepeatingSectionRequest);
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"An error occurred while adding a repeating section on {formId} - {pageId} for applicant {applicantId}", ex);
        } 
    }

    private async Task<T> GetApiResponseAsync<T>(string apiUrl)
    {
        return await _retryPolicy.ExecuteAsync(async () =>
        {
            var client = _httpClientFactory.CreateClient();
            var baseAddress = _configuration["FormAPI:BaseAddress"];
            if (string.IsNullOrWhiteSpace(baseAddress))
            {
                throw new ApplicationException("The base address is not configured.");
            }
            client.BaseAddress = new Uri(baseAddress);
            var response = await client.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(responseString))
            {
                throw new ApplicationException("The response content is empty.");
            }

            var responseObject = _safeJsonHelper.SafeDeserializeObject<T>(responseString);
            if (responseObject == null)
            {
                throw new ApplicationException("Failed to deserialize the response data.");
            }

            return responseObject;
        });
    }

    private async Task<T> PostApiResponseAsync<T>(string apiUrl, object requestModel)
    {
        return await _retryPolicy.ExecuteAsync(async () =>
        {
            var client = _httpClientFactory.CreateClient();
            var baseAddress = _configuration["FormAPI:BaseAddress"];
            if (string.IsNullOrWhiteSpace(baseAddress))
            {
                throw new ApplicationException("The base address is not configured.");
            }
            client.BaseAddress = new Uri(baseAddress);
            var response = await client.PostAsJsonAsync(apiUrl, requestModel);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(responseString))
            {
                throw new ApplicationException("The response content is empty.");
            }

            var responseObject = _safeJsonHelper.SafeDeserializeObject<T>(responseString);
            if (responseObject == null)
            {
                throw new ApplicationException("Failed to deserialize the response data.");
            }

            return responseObject;
        });
    }

    public async Task<UploadFileResponse> StreamFileToExternalApi(IFormFile file, string uploadEndpoint)
    {
        using var fileStream = file.OpenReadStream();
        using var content = new StreamContent(fileStream);
        content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);

        using var multipartContent = new MultipartFormDataContent();
        multipartContent.Add(content, "file", file.FileName);

        var client = _httpClientFactory.CreateClient();
        var response = await client.PostAsync(uploadEndpoint, multipartContent);
        response.EnsureSuccessStatusCode();

        return JsonConvert.DeserializeObject<UploadFileResponse>(await response.Content.ReadAsStringAsync());
    }
}