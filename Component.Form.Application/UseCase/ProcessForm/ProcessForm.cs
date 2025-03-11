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
        
        RepeatingModel repeatingModel = null;
        if (currentPage.Repeating) 
        {
            repeatingModel = JsonConvert.DeserializeObject<RepeatingModel>(request.Form[currentPage.RepeatKey]);
        }

        if (currentPage.Repeating)
        {
            UpdateRepeatingData(currentPage, request, data, repeatingModel);
        }
        else
        {
            var newData = GetData(currentPage, request.Form);
            foreach (var item in newData)
            {
                ((IDictionary<string, object>)data)[item.Key] = item.Value;
            }
        }

        var errors = new Dictionary<string, List<string>>();

        // Loop through questions and validate them
        foreach (var question in currentPage.Components.Where(c => c.IsQuestionType))
        {
            string inputName = question.Name;
            IComponentHandler handler = _componentHandlerFactory.GetFor(question.Type);

            // Evaluate validation rules
            if (!question.Optional)
            {
                var validationResult = await handler.Validate(inputName, data, question.ValidationRules, currentPage.Repeating, currentPage.RepeatKey);
                if (validationResult.Count > 0)
                {
                    errors.Add(inputName, validationResult);
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
                FormData = JsonConvert.SerializeObject(data),
                RepeatIndex = repeatingModel?.RepeatIndex ?? -1
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
            int repeatIndex = -1;
            bool metCondition = false;

            foreach (var condition in currentPage.Conditions)
            {
                if (await ExpressionHelper.EvaluateCondition(condition.Expression, data))
                {
                    nextPageId = condition.NextPageId;
                    metCondition = true;

                    if (currentPage.Repeating)
                    {
                        repeatIndex = repeatingModel.RepeatIndex + 1;
                    }
                }
            }

            if (metCondition)
            {
                return new ProcessFormResponseModel()
                {
                    NextPage = nextPageId,
                    RepeatIndex = repeatIndex
                };
            }
        }

        if (currentPage.Repeating)
        {
            return new ProcessFormResponseModel()
            {
                NextPage = currentPage.NextPageId,
                RepeatIndex = repeatingModel.RepeatIndex
            };
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

    public dynamic GetData(Page currentPage, Dictionary<string, string> newData) 
    {
        dynamic data = new ExpandoObject();

        foreach (var question in currentPage.Components.Where(c => c.IsQuestionType))
        {
            string inputName = question.Name;

            if (newData.ContainsKey(inputName))
            {
                IComponentHandler handler = _componentHandlerFactory.GetFor(question.Type);

                if (handler.GetDataType().Equals(ComponentHandlerFactory.GetDataType(typeof(string)))) 
                {
                    ((IDictionary<string, object>)data)[inputName] = newData[inputName];
                }
                else 
                {
                    ((IDictionary<string, object>)data)[inputName] = ParseData(newData[inputName]);
                }
            }
        }

        return data;
    }

    private void UpdateRepeatingData(Page currentPage, ProcessFormRequestModel request, dynamic data, RepeatingModel repeatingModel)
    {
        var newData = GetData(currentPage, repeatingModel.FormData);

        var dataAsDictionary = (IDictionary<string, object>)data;

        if (dataAsDictionary.ContainsKey(currentPage.RepeatKey))
        {
            var repeatData = (List<object>)dataAsDictionary[currentPage.RepeatKey];
            int repeatIndex = repeatingModel.RepeatIndex;

            if (repeatIndex < repeatData.Count)
            {
                var existingData = repeatData[repeatIndex];
                foreach (var item in newData)
                {
                    ((IDictionary<string, object>)existingData)[item.Key] = item.Value;
                }
                repeatData[repeatIndex] = existingData;
            }
            else
            {
                var newRepeatData = new ExpandoObject();
                foreach (var item in newData)
                {
                    ((IDictionary<string, object>)newRepeatData)[item.Key] = item.Value;
                }
                repeatData.Add(newRepeatData);
            }

            dataAsDictionary[currentPage.RepeatKey] = repeatData;
        }
        else
        {
            var repeatData = new List<ExpandoObject>();
            var newRepeatData = new ExpandoObject();
            foreach (var item in newData)
            {
                ((IDictionary<string, object>)newRepeatData)[item.Key] = item.Value;
            }
            repeatData.Add(newRepeatData);
            dataAsDictionary[currentPage.RepeatKey] = (IEnumerable<ExpandoObject>)repeatData;
        }
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