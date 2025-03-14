using System;
using Component.Form.Model;
using Newtonsoft.Json;
using Component.Form.UI.Services.Model;

namespace Component.Form.UI.Services;

public class FormAPIService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private const string GetFormApiUrl = "api/getForm/";
    private const string GetFormDataApiUrl = "api/getData/";
    private const string GetFormDataForPageApiUrl = "api/getDataForPage/";
    private const string ProcessFormApiUrl = "api/processForm/";
    private const string UpdateFormApiUrl = "api/updateForm/";

    public FormAPIService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
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
            throw new ApplicationException("An error occurred while getting form data.", ex);
        }
    }

    public async Task<GetDataResponseModel> GetFormDataAsync(string applicantId)
    {
        try
        {
            var dataModel = await GetApiResponseAsync<GetDataResponseModel>($"{GetFormDataApiUrl}{applicantId}");
            return dataModel;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("An error occurred while getting form data.", ex);
        }
    }

    public async Task<GetDataForPageResponseModel> GetFormDataForPageAsync(string formId, string pageId, string applicantId, int repeatIndex)
    {
        try
        {
            return await GetApiResponseAsync<GetDataForPageResponseModel>($"{GetFormDataForPageApiUrl}{formId}/{pageId}/{applicantId}/{repeatIndex}");
        }
        catch (Exception ex)
        {
            throw new ApplicationException("An error occurred while getting form data.", ex);
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
            throw new ApplicationException("An error occurred while processing form data.", ex);
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

        private async Task<T> GetApiResponseAsync<T>(string apiUrl)
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

        var responseObject = JsonConvert.DeserializeObject<T>(responseString);
        if (responseObject == null)
        {
            throw new ApplicationException("Failed to deserialize the response data.");
        }

        return responseObject;
    }

    private async Task<T> PostApiResponseAsync<T>(string apiUrl, object requestModel)
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

        var responseObject = JsonConvert.DeserializeObject<T>(responseString);
        if (responseObject == null)
        {
            throw new ApplicationException("Failed to deserialize the response data.");
        }

        return responseObject;
    }
}