using System;
using Component.Core.Application;
using Component.Form.Application.UseCase.GetData.Model;
using Component.Form.Application.UseCase.ProcessForm.Infrastructure;

namespace Component.Form.Application.UseCase.GetData;

public class GetData : IRequestResponseUseCase<GetDataRequestModel,GetDataResponseModel>
{
    private readonly IFormDataStore _formDataStore;

    public GetData(IFormDataStore formDataStore)
    {
        _formDataStore = formDataStore;
    }

    public async Task<GetDataResponseModel> HandleAsync(GetDataRequestModel request)
    {
        try
        {
            var formData = await _formDataStore.GetFormDataAsync(request.ApplicantId);
            return new GetDataResponseModel { FormData = formData };
        }
        catch (Exception ex)
        {
            // Handle exception (log it, rethrow it, or return a default value)
            throw new ApplicationException($"An error occurred while getting form data for applicant {request.ApplicantId}", ex);
        }
    }
}
