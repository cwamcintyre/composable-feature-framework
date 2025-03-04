using Component.Core.Application;
using Component.Form.Application.UseCase.GetForm.Infrastructure;
using Component.Form.Application.UseCase.ProcessForm.Infrastructure;
using Component.Form.Application.UseCase.ProcessForm.Model;
using Component.Form.Model.ComponentHandler;
using System.Dynamic;
using Newtonsoft.Json;
using Component.Form.Model;
namespace Component.Form.Application.UseCase.ProcessForm;

public class ProcessForm : IRequestResponseUseCase<ProcessFormRequestModel, ProcessFormResponseModel>
{
    private readonly IFormStore _formStore;
    private readonly IFormDataStore _formDataStore;
    private readonly ComponentHandlerFactory _componentHandlerFactory;
    
    public ProcessForm(IFormStore formStore, IFormDataStore formDataStore, ComponentHandlerFactory componentHandlerFactory)
    {
        _formStore = formStore;
        _formDataStore = formDataStore;
        _componentHandlerFactory = componentHandlerFactory;
    }

    public async Task<ProcessFormResponseModel> HandleAsync(ProcessFormRequestModel request)
    {
        var form = await _formStore.GetFormAsync(request.FormId);
        var formDataModel = await _formDataStore.GetFormDataAsync(request.ApplicantId);

        var formData = formDataModel.Data;
        var routeData = formDataModel.Route;

        dynamic data = ParseData(formData);

        if (form == null)
        {
            throw new ArgumentException($"Form {request.FormId} not found");
        }

        var currentPage = form.Pages.FirstOrDefault(p => p.PageId == request.PageId);

        if (currentPage == null) return new ProcessFormResponseModel 
        { 
            Errors = new Dictionary<string, List<string>> { { "Page", new List<string>() { "Page not found" } } }
        };

        var errors = new Dictionary<string, List<string>>();

        // Loop through questions and validate them
        foreach (var question in currentPage.Components.Where(c => c.IsQuestionType))
        {
            string inputName = question.Name;

            if (request.Form.ContainsKey(inputName))
            {
                IComponentHandler handler = _componentHandlerFactory.GetFor(question.Type);

                if (handler.GetDataType().Equals(ComponentHandlerFactory.GetDataType(typeof(string)))) 
                {
                    ((IDictionary<string, object>)data)[inputName] = request.Form[inputName];
                }
                else 
                {
                    ((IDictionary<string, object>)data)[inputName] = ParseData(request.Form[inputName]);
                }

                // Evaluate validation rules
                if (!question.Optional) 
                {
                    var validationResult = await handler.Validate(inputName, data, question.ValidationRules);
                    if (validationResult.Count > 0)
                    {
                        errors.Add(inputName, validationResult);
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
                FormData = JsonConvert.SerializeObject(data)
            };
        }

        // add the current page ID to the route..
        routeData.Push(currentPage.PageId);

        // store the current form data.
        await _formDataStore.SaveFormDataAsync(request.FormId, request.ApplicantId, JsonConvert.SerializeObject(data), routeData);

        // If there is a condition to move to the next page, evaluate it.
        // else move to the next page or submit the form if on the last page
        if (currentPage.Conditions != null)
        {
            var nextPageId = "";
            bool metCondition = false;

            foreach (var condition in currentPage.Conditions)
            {
                if (await ExpressionHelper.EvaluateCondition(condition.Expression, data))
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

    public dynamic ParseData(string formData) 
    {
        var jsonSettings = GetJsonSerializerSettings();
        var data = JsonConvert.DeserializeObject<ExpandoObject>(formData, jsonSettings);
        
        if (data == null) 
        {
            data = new ExpandoObject();
        }

        return data;
    }

    private JsonSerializerSettings GetJsonSerializerSettings()
    {
        return new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.None,
            SerializationBinder = new SafeTypeResolver(_componentHandlerFactory.GetAllTypes())
        };
    }
}