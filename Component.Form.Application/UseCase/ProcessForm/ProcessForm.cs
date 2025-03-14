using Component.Core.Application;
using Component.Form.Application.Shared.Infrastructure;
using Component.Form.Application.UseCase.ProcessForm.Model;
using Component.Form.Model.ComponentHandler;
using System.Dynamic;
using Newtonsoft.Json;
using Component.Form.Model;
using Component.Form.Application.Helpers;
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

        dynamic data = FormHelper.ParseData(formData, _componentHandlerFactory);

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
            var newData = FormHelper.GetData(currentPage, request.Form, _componentHandlerFactory);
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
                var validationResult = await handler.Validate(inputName, data, question.ValidationRules, currentPage.Repeating, currentPage.RepeatKey, repeatingModel?.RepeatIndex ?? 0);
                if (validationResult.Count > 0)
                {
                    errors.Add(inputName, validationResult);
                }
            }
        }

        // store the current form data. errors and all...
        await _formDataStore.SaveFormDataAsync(request.FormId, request.ApplicantId, JsonConvert.SerializeObject(data), routeData);

        // If there are validation errors, return to the same page
        if (errors.Any())
        {
            return new ProcessFormResponseModel
            {
                Errors = errors,
                NextPage = currentPage.PageId,
                RepeatIndex = repeatingModel?.RepeatIndex ?? 0
            };
        }

        // If there is a condition to move to the next page, evaluate it.
        // else move to the next page or submit the form if on the last page
        var conditionMetResult = await FormHelper.MeetsCondition(currentPage, data, repeatingModel?.RepeatIndex ?? 0);
        if (conditionMetResult.MetCondition)
        {                        
            return new ProcessFormResponseModel()
            {
                NextPage = conditionMetResult.NextPageId,
                RepeatIndex = conditionMetResult.RepeatIndex
            };
        }
        else
        {
            // check that the user has not gone back to a previous page and not met the condition to repeat the page
            // there may be extraneous data after the current page that needs to be removed
            if (currentPage.Repeating && currentPage.RepeatEnd) 
            {
                var dataAsDictionary = (IDictionary<string, object>)data;
                var repeatingData = dataAsDictionary[currentPage.RepeatKey];
                var repeatDataList = (List<object>)repeatingData;
                if (repeatDataList.Count - 1 > repeatingModel.RepeatIndex)
                {
                    repeatDataList.RemoveRange(repeatingModel.RepeatIndex + 1, repeatDataList.Count - (repeatingModel.RepeatIndex + 1));
                    dataAsDictionary[currentPage.RepeatKey] = repeatDataList;
                    // save again...
                    await _formDataStore.SaveFormDataAsync(request.FormId, request.ApplicantId, JsonConvert.SerializeObject(data), routeData);
                }
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

    private void UpdateRepeatingData(Page currentPage, ProcessFormRequestModel request, dynamic data, RepeatingModel repeatingModel)
    {
        var newData = FormHelper.GetData(currentPage, repeatingModel.FormData, _componentHandlerFactory);

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
}