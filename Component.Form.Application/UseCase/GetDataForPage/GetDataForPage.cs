using System;
using Component.Core.Application;
using Component.Form.Application.UseCase.GetDataForPage.Model;
using Component.Form.Application.Shared.Infrastructure;
using Component.Form.Model.ComponentHandler;
using Component.Form.Application.Helpers;

namespace Component.Form.Application.UseCase.GetData;

public class GetDataForPage : IRequestResponseUseCase<GetDataForPageRequestModel,GetDataForPageResponseModel>
{
    private readonly IFormStore _formStore;
    private readonly IFormDataStore _formDataStore;
    private readonly ComponentHandlerFactory _componentHandlerFactory;

    public GetDataForPage(IFormDataStore formDataStore, IFormStore formStore, ComponentHandlerFactory componentHandlerFactory)
    {
        _formStore = formStore;
        _formDataStore = formDataStore;
        _componentHandlerFactory = componentHandlerFactory;
    }   

    public async Task<GetDataForPageResponseModel> HandleAsync(GetDataForPageRequestModel request)
    {
        var formData = await _formDataStore.GetFormDataAsync(request.ApplicantId);
        var form = await _formStore.GetFormAsync(request.FormId);
        
        if (formData == null)
        {
            throw new ArgumentException($"Form data for {request.ApplicantId} not found");
        }

        if (form == null)
        {
            throw new ArgumentException($"Form {request.FormId} not found");
        }

        var page = form.Pages.FirstOrDefault(p => p.PageId == request.PageId);

        if (page == null)
        {
            throw new ArgumentException($"Page {request.PageId} not found");
        }

        var parsedData = FormHelper.ParseData(formData.Data, _componentHandlerFactory);
        var parsedDataAsDictionary = (IDictionary<string, object>)parsedData;

        var errors = new Dictionary<string, List<string>>();

        // Loop through questions and validate them
        foreach (var question in page.Components.Where(c => c.IsQuestionType))
        {
            string inputName = question.Name;
            IComponentHandler handler = _componentHandlerFactory.GetFor(question.Type);

            // Evaluate validation rules
            if (!question.Optional && (parsedDataAsDictionary.ContainsKey(inputName) || (page.Repeating && parsedDataAsDictionary.ContainsKey(page.RepeatKey))))
            {
                var validationResult = await handler.Validate(inputName, parsedData, question.ValidationRules, page.Repeating, page.RepeatKey, request.RepeatIndex);
                if (validationResult.Count > 0)
                {
                    errors.Add(inputName, validationResult);
                }
            }
        }
  
        var forThisPage = FormHelper.GetDataForPage(page, parsedDataAsDictionary);

        var previousPage = await FormHelper.CalculatePreviousPage(form, request.PageId, parsedData, request.RepeatIndex);

        var response = new GetDataForPageResponseModel
        {
            Errors = errors,
            FormData = forThisPage,
            PreviousPage = previousPage.PageId,
            PreviousRepeatIndex = previousPage.RepeatIndex
        };

        return response;
    }
}
