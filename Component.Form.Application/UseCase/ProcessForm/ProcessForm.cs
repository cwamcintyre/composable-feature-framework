using System;
using System.Diagnostics;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Component.Core.Application;
using Component.Form.Application.UseCase.GetForm.Infrastructure;
using Component.Form.Application.UseCase.ProcessForm.Infrastructure;
using Component.Form.Application.UseCase.ProcessForm.Model;
namespace Component.Form.Application.UseCase.ProcessForm;

public class ProcessForm : IRequestResponseUseCase<ProcessFormRequestModel, ProcessFormResponseModel>
{
    private readonly IFormStore _formStore;
    private readonly IFormDataStore _formDataStore;

    public ProcessForm(IFormStore formStore, IFormDataStore formDataStore)
    {
        _formStore = formStore;
        _formDataStore = formDataStore;
    }

    public async Task<ProcessFormResponseModel> HandleAsync(ProcessFormRequestModel request)
    {
        var form = await _formStore.GetFormAsync(request.FormId);
        var formDataModel = await _formDataStore.GetFormDataAsync(request.ApplicantId);

        var formData = formDataModel.Data;
        var routeData = formDataModel.Route;

        if (form == null)
        {
            throw new ArgumentException($"Form {request.FormId} not found");
        }

        var currentPage = form.Pages.FirstOrDefault(p => p.PageId == request.PageId);

        if (currentPage == null) return new ProcessFormResponseModel 
        { 
            Errors = new Dictionary<string, string> { { "Page", "Page not found" } }
        };

        var errors = new Dictionary<string, string>();

        // Loop through questions and validate them
        var thisPageData = new Dictionary<string, string>();
        foreach (var question in currentPage.Components.Where(c => c.IsQuestionType))
        {
            string inputName = question.Name;

            // Check if the required field is filled
            if (question.Required && !request.Form.ContainsKey(inputName) && question.Type != "file")
            {
                errors.Add(inputName, $"The field '{question.Label}' is required.");
                continue;
            }

            if (request.Form.ContainsKey(inputName))
            {
                formData[inputName] = request.Form[inputName];
                thisPageData[inputName] = request.Form[inputName];

                // Evaluate validation rules
                if (question.ValidationRules != null) 
                {
                    foreach (var rule in question.ValidationRules)
                    {
                        var validationResult = await EvaluateCondition(rule.Expression, formData);
                        if (!validationResult)
                        {
                            errors.Add(inputName, rule.ErrorMessage);
                        }
                    }
                }
            }
        }

        // If there are validation errors, return to the same page
        if (errors.Any())
        {
            return new ProcessFormResponseModel
            {
                Errors = errors,
                NextPage = currentPage.PageId,
                FormData = thisPageData
            };
        }

        // add the current page ID to the route..
        routeData.Push(currentPage.PageId);

        // store the current form data.
        await _formDataStore.SaveFormDataAsync(request.FormId, request.ApplicantId, formData, routeData);

        // If there is a condition to move to the next page, evaluate it.
        // else move to the next page or submit the form if on the last page
        if (currentPage.Conditions != null)
        {
            var nextPageId = "";
            bool metCondition = false;

            foreach (var condition in currentPage.Conditions)
            {
                if (await EvaluateCondition(condition.Expression, formData))
                {
                    nextPageId = condition.NextPageId;
                    metCondition = true;
                }
            }

            if (metCondition) 
            {
                return new ProcessFormResponseModel()
                {
                    NextPage = nextPageId
                };
            }
        }

        return new ProcessFormResponseModel()
        {
            NextPage = currentPage.NextPageId
        };
    }

    private async Task<bool> EvaluateCondition(string expression, Dictionary<string, string> formData)
    {
        var options = ScriptOptions.Default.AddImports("System", "System.Collections.Generic");

        var script = CSharpScript.Create<bool>(expression, options, typeof(Globals));
        var result = await script.RunAsync(new Globals { FormData = formData });
        return result.ReturnValue;
    }
}

public class Globals
{
    public Dictionary<string, string> FormData { get; set; }
}