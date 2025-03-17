using System.Dynamic;
using Component.Form.Application.ComponentHandler;
using Component.Form.Application.Helpers;
using Component.Form.Application.Shared.Infrastructure;
using Component.Form.Application.UseCase.GetDataForPage.Model;
using Component.Form.Application.UseCase.ProcessForm.Model;
using Component.Form.Model;
using Component.Form.Model.PageHandler;

namespace Component.Form.Application.PageHandler.InlineRepeating;

public class InlineRepeatingPageHandler : IPageHandler
{
    private readonly IFormStore _formStore;
    private readonly IFormDataStore _formDataStore;
    private readonly ComponentHandlerFactory _componentHandlerFactory;
    private readonly SafeJsonHelper _safeJsonHelper;
    
    public InlineRepeatingPageHandler(IFormStore formStore, IFormDataStore formDataStore, ComponentHandlerFactory componentHandlerFactory, SafeJsonHelper safeJsonHelper)
    {
        _formStore = formStore;
        _formDataStore = formDataStore;
        _componentHandlerFactory = componentHandlerFactory;
        _safeJsonHelper = safeJsonHelper;
    }

    public bool IsFor(string type)
    {
        return type.Equals("inlineRepeating", StringComparison.OrdinalIgnoreCase);
    }

    public static string GetSafeType()
    {
        return SafeJsonHelper.GetSafeType(typeof(InlineRepeatingPageSection));
    }

    public async Task<GetDataForPageResponseModel> Get(PageBase page, dynamic data, string extraData)
    {
        var repeatingData = (InlineRepeatingData)extraData;

        InlineRepeatingPageSection section = (InlineRepeatingPageSection)page;

        InlineRepeatingPage repeatPage = null;

        // if no PageId is provided, assume the start page.
        if (!String.IsNullOrEmpty(repeatingData.PageId))
        {
            repeatPage = section.RepeatingPages.Find(p => p.PageId == repeatingData.PageId);
        }
        else 
        {
            repeatPage = section.RepeatingPages.Find(p => p.RepeatStart);
        }
        
        if (repeatPage == null)
        {
            throw new ArgumentException($"Could not find repeating page with id {repeatingData.PageId}");
        }

        var parsedDataAsDictionary = (IDictionary<string, object>)data;

        var errors = new Dictionary<string, List<string>>();

        var forThisPage = new Dictionary<string, object>();

        List<object> repeatDataList = null;
        if (parsedDataAsDictionary.ContainsKey(section.RepeatKey))
        {
            repeatDataList = (List<object>)parsedDataAsDictionary[section.RepeatKey];
        }
        else
        {
            repeatDataList = new List<object>();
        }

        IDictionary<string, object> repeatData = null;
        if (repeatingData.RepeatIndex < repeatDataList.Count)
        {
            repeatData = (IDictionary<string, object>)repeatDataList[repeatingData.RepeatIndex];
        }
        else
        {
            repeatData = new ExpandoObject();
        }

        foreach (var question in repeatPage.Components.Where(c => c.IsQuestionType))
        {
            string inputName = question.Name;
            IComponentHandler handler = _componentHandlerFactory.GetFor(question.Type);

            if (repeatData.ContainsKey(inputName))
            {
                forThisPage.Add(inputName, repeatData[inputName]);

                List<string> validationResult = await handler.Validate(inputName, parsedDataAsDictionary, question.ValidationRules, true, section.RepeatKey, repeatingData.RepeatIndex);
                if (validationResult.Count > 0)
                {
                    errors.Add(inputName, validationResult);
                }
            }
        }

        return new GetDataForPageResponseModel
        {
            FormData = new Dictionary<string, object>()
            {
                { section.RepeatKey, forThisPage }
            },
            Errors = errors,
        };
        
    }

    public async Task<ProcessFormResponseModel> Process(PageBase page, dynamic existingData, Dictionary<string, string> formData)
    {
        InlineRepeatingPageSection section = (InlineRepeatingPageSection)page;
        
        var repeatIndex = Convert.ToInt32(formData["RepeatIndex"]);
        var repeatPageId = formData["RepeatPageId"];
        var repeatingPageData = new InlineRepeatingData()
        {
            PageId = repeatPageId,
            RepeatIndex = repeatIndex
        };

        InlineRepeatingPage repeatPage = section.RepeatingPages.Find(p => p.PageId == repeatPageId);
        
        if (repeatPage == null)
        {
            throw new ArgumentException($"Could not find repeating page with id {repeatPageId}");
        }

        var errors = new Dictionary<string, List<string>>();

        var repeatingModelString = formData[section.RepeatKey];
        var repeatingModel = _safeJsonHelper.SafeDeserializeObject<RepeatingModel>(repeatingModelString);

        UpdateRepeatingData(section, new ProcessFormRequestModel()
        {
            PageId = repeatPageId,
            Form = formData
        }, existingData, repeatingModel);

        // Loop through questions and validate them
        foreach (var question in repeatPage.Components.Where(c => c.IsQuestionType))
        {
            string inputName = question.Name;
            IComponentHandler handler = _componentHandlerFactory.GetFor(question.Type);

            // Evaluate validation rules
            if (!question.Optional)
            {
                var validationResult = await handler.Validate(inputName, existingData, question.ValidationRules, true, section.RepeatKey, repeatIndex);
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
                Data = existingData,
                NextPageId = page.PageId,
                ExtraData = repeatingPageData
            };
        }

        var nextPageResult = await GetNextPageId(page, existingData, (string)repeatingPageData);
        return new ProcessFormResponseModel
        {
            Data = existingData,
            NextPageId = nextPageResult.NextPageId,
            ExtraData = nextPageResult.ExtraData
        };
    }

    public async Task<NextPageIdResult> GetNextPageId(PageBase page, dynamic data, string extraData)
    {
        InlineRepeatingPageSection section = (InlineRepeatingPageSection)page;
        InlineRepeatingData repeatingData = (InlineRepeatingData)extraData;
        
        InlineRepeatingPage repeatPage = null;

        if (!String.IsNullOrEmpty(repeatingData.PageId))
        {
            repeatPage = section.RepeatingPages.Find(p => p.PageId == repeatingData.PageId);
        }
        else 
        {
            repeatPage = section.RepeatingPages.Find(p => p.RepeatStart);
        }
        
        if (repeatPage == null)
        {
            throw new ArgumentException($"Could not find repeating page with id {repeatingData.PageId}");
        }

        var conditionMetResult = await MeetsCondition(repeatPage, data, ((InlineRepeatingData)extraData).RepeatIndex);
        if (conditionMetResult.MetCondition)
        {
            return new NextPageIdResult()
            {
                NextPageId = section.PageId,
                ExtraData = $"{conditionMetResult.RepeatIndex}-{conditionMetResult.NextPageId}"
            };
        }

        if (repeatPage.RepeatEnd)
        {
            return new NextPageIdResult()
            {
                NextPageId = section.NextPageId
            };
        }
        else 
        {
            return new NextPageIdResult()
            {
                NextPageId = section.PageId,
                ExtraData = $"{repeatingData.RepeatIndex}-{repeatPage.NextPageId}"
            };
        }
    }

    public static async Task<ConditionResult> MeetsCondition(PageBase currentPage, IDictionary<string, object> data, int repeatingIndex)
    {
        if (currentPage.Conditions != null)
        {
            var nextPageId = "";
            int returnRepeatIndex = 0;
            bool metCondition = false;

            foreach (var condition in currentPage.Conditions)
            {
                if (await ExpressionHelper.EvaluateCondition(condition.Expression, data, repeatingIndex))
                {
                    nextPageId = condition.NextPageId;
                    metCondition = true;

                    returnRepeatIndex = repeatingIndex + 1;
                }
            }

            if (metCondition)
            {
                return new ConditionResult()
                {
                    MetCondition = true,
                    NextPageId = nextPageId,
                    RepeatIndex = returnRepeatIndex
                };
            }
        }

        return new ConditionResult()
        {
            MetCondition = false
        };
    }

    private Dictionary<string, object> GetData(PageBase currentPage, Dictionary<string, string> newData, ComponentHandlerFactory _componentHandlerFactory) 
    {
        var data = new Dictionary<string, object>();

        foreach (var question in currentPage.Components.Where(c => c.IsQuestionType))
        {
            string inputName = question.Name;

            if (newData.ContainsKey(inputName))
            {
                IComponentHandler handler = _componentHandlerFactory.GetFor(question.Type);

                if (handler.GetDataType().Equals(SafeJsonHelper.GetSafeType(typeof(string)))) 
                {
                    data[inputName] = newData[inputName];
                }
                else 
                {
                    data[inputName] = FormHelper.ParseData(newData[inputName], _componentHandlerFactory);
                }
            }
        }

        return data;
    }

    private void UpdateRepeatingData(InlineRepeatingPageSection section, ProcessFormRequestModel request, dynamic data, RepeatingModel repeatingModel)
    {
        var currentPage = section.RepeatingPages.Find(p => p.PageId == request.PageId);
        if (currentPage == null)
        {
            throw new ArgumentException($"Could not find repeating page with id {request.PageId}");
        }

        var newData = GetData(currentPage, repeatingModel.FormData, _componentHandlerFactory);

        var dataAsDictionary = (IDictionary<string, object>)data;

        if (dataAsDictionary.ContainsKey(section.RepeatKey))
        {
            var repeatData = (List<object>)dataAsDictionary[section.RepeatKey];
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

            dataAsDictionary[section.RepeatKey] = repeatData;
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
            dataAsDictionary[section.RepeatKey] = (IEnumerable<ExpandoObject>)repeatData;
        }
    }
}

public class InlineRepeatingData
{
    public string? PageId { get; set; }
    public int RepeatIndex { get; set; }

    public static implicit operator InlineRepeatingData(string data)
    {
        if (string.IsNullOrEmpty(data))
        {
            // if no data is provided, assume the start page with index 0
            return new InlineRepeatingData()
            {
                PageId = null,
                RepeatIndex = 0
            };
        }

        var split = data.Split('-', 2);
        
        if (split.Length != 2)
        {
            throw new ArgumentException($"Invalid InlineRepeatingData format - expected repeatIndex/pageId, got {data}");
        }

        return new InlineRepeatingData
        {
            PageId = split[1],
            RepeatIndex = Convert.ToInt32(split[0])
        };
    }

    public static implicit operator string(InlineRepeatingData data)
    {
        return $"{data.RepeatIndex}-{data.PageId}";
    }
}
