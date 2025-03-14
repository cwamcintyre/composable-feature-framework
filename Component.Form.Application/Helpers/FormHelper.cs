using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using Component.Form.Model;
using Component.Form.Model.ComponentHandler;
using Newtonsoft.Json;

namespace Component.Form.Application.Helpers;

public static class FormHelper
{
    public static dynamic ParseData(string formData, ComponentHandlerFactory _componentHandlerFactory) 
    {
        var jsonSettings = GetJsonSerializerSettings(_componentHandlerFactory);
        var data = JsonConvert.DeserializeObject<ExpandoObject>(formData, jsonSettings);
        
        if (data == null) 
        {
            data = new ExpandoObject();
        }

        return data;
    }

    public static dynamic GetData(Page currentPage, Dictionary<string, string> newData, ComponentHandlerFactory _componentHandlerFactory) 
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
                    ((IDictionary<string, object>)data)[inputName] = ParseData(newData[inputName], _componentHandlerFactory);
                }
            }
        }

        return data;
    }

    public static Dictionary<string, object> GetDataForPage(Page page, IDictionary<string, object> data, bool onlyForRepeatIndex = false, int repeatIndex = 0)
    {
        var forThisPage = new Dictionary<string, object>();

        if (page.Repeating)
        {
            if (data.ContainsKey(page.RepeatKey))
            {
                if (onlyForRepeatIndex)
                {
                    var repeatDataList = (List<object>)data[page.RepeatKey];
                    if (repeatDataList.Count > repeatIndex)
                    {
                        forThisPage.Add(page.RepeatKey, new List<object> { repeatDataList[repeatIndex] });
                    }
                }
                else 
                {
                    forThisPage.Add(page.RepeatKey, data[page.RepeatKey]);
                }
            }
        }
        else 
        {
            foreach (var question in page.Components.Where(c => c.IsQuestionType))
            {
                string inputName = question.Name;

                if (data.ContainsKey(inputName))
                {
                    forThisPage.Add(inputName, data[inputName]);
                }
            }
        }

        return forThisPage;
    }

    public static IDictionary<string, object> MergeWalkedData(IDictionary<string, object> withMe, Dictionary<string, object> toMerge, int repeatIndex)
    {
        foreach (var item in toMerge)
        {
            if (item.Value is List<object>)
            {
                var repeatList = (List<object>)item.Value;
                
                if (!withMe.ContainsKey(item.Key))
                {
                    withMe.Add(item.Key, new List<object>());
                }

                var withMeRepeatList = (List<object>)withMe[item.Key];

                if (withMeRepeatList.Count <= repeatIndex)
                {
                    withMeRepeatList.Add(repeatList[0]);
                }                

                continue;
            }

            withMe[item.Key] = item.Value;
        }

        return withMe;
    }

    public static JsonSerializerSettings GetJsonSerializerSettings(ComponentHandlerFactory _componentHandlerFactory)
    {
        return new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.None,
            SerializationBinder = new SafeTypeResolver(_componentHandlerFactory.GetAllTypes())
        };
    }

    public static async Task<ConditionResult> MeetsCondition(Page currentPage, IDictionary<string, object> data, int repeatingIndex)
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

                    if (currentPage.Repeating)
                    {
                        returnRepeatIndex = repeatingIndex + 1;
                    }
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

        return new ConditionResult { MetCondition = false };
    }

    // considering that the user has the back link and the back button (and we cannot effectively test for the back button), we have to calculate the previous page
    // based on first principles every time a page is loaded.
    public static async Task<PreviousPageModel> CalculatePreviousPage(FormModel formModel, string currentPageId, IDictionary<string, object> data, int repeatIndex)
    {
        // calculate forward journey up until now...
        var pages = new Stack<PageItemModel>();

        await CalculatePreviousPageRecursive(formModel, formModel.Pages[0], currentPageId, data, pages, repeatIndex);

        if (pages.Count == 0)
        {
            return new PreviousPageModel
            {
                PageId = "",
                RepeatIndex = 0
            };
        }

        return new PreviousPageModel
        {
            PageId = pages.Peek().PageId,
            RepeatIndex = pages.Peek().RepeatIndex
        };
    }

    private static async Task CalculatePreviousPageRecursive(
        FormModel formModel,
        Page page, 
        string currentPageId, 
        IDictionary<string, object> data, 
        Stack<PageItemModel> pages,
        int repeatIndex, 
        IDictionary<string, object> walkedData = null, 
        int walkedRepeatIndex = 0)
    {
        if (page.PageId == currentPageId && repeatIndex == walkedRepeatIndex)
        {
            return;
        }   

        pages.Push(new PageItemModel { PageId = page.PageId, RepeatIndex = walkedRepeatIndex });
        
        if (walkedData == null)
        {
            walkedData = new ExpandoObject();
        }

        walkedData = MergeWalkedData(walkedData, GetDataForPage(page, data, true, walkedRepeatIndex), walkedRepeatIndex);

        var nextPage = formModel.Pages.FirstOrDefault(c => c.PageId == page.NextPageId);

        if (page.Repeating && page.RepeatEnd) 
        {
            // if repeating end, calculate condition...
            var conditionResult = await MeetsCondition(page, walkedData, walkedRepeatIndex);

            if (conditionResult.MetCondition)
            {
                // if the condition is met and we're repeating the section, we'll use the repeat index from the condition result rather than walkedRepeatIndex
                // which we may have reset..
                await CalculatePreviousPageRecursive(formModel, formModel.Pages.FirstOrDefault(c => c.PageId == conditionResult.NextPageId), currentPageId, data, pages, repeatIndex, walkedData, conditionResult.RepeatIndex);
            }
            else 
            {
                // check whether the length of the repeat data is the same as the repeat index, if so, we are at the end of the repeat data
                // and we should move to the next page.
                var repeatData = (List<object>)walkedData[page.RepeatKey];
                var realRepeatData = (List<object>)data[page.RepeatKey];
                if (repeatData.Count == realRepeatData.Count)
                {
                    walkedRepeatIndex = 0; // reset walked repeat index
                }

                await CalculatePreviousPageRecursive(formModel, nextPage, currentPageId, data, pages, repeatIndex, walkedData, walkedRepeatIndex);
            }
        }
        else 
        {
            // if page condition, calculate..
            var conditionResult = await MeetsCondition(page, walkedData, walkedRepeatIndex);

            if (conditionResult.MetCondition)
            {
                await CalculatePreviousPageRecursive(formModel, formModel.Pages.FirstOrDefault(c => c.PageId == conditionResult.NextPageId), currentPageId, data, pages, repeatIndex, walkedData, conditionResult.RepeatIndex);
            }

            // else just skip to next page..
            await CalculatePreviousPageRecursive(formModel, nextPage, currentPageId, data, pages, repeatIndex, walkedData, walkedRepeatIndex);
        }
    }
}

public class PreviousPageModel 
{
    public string PageId { get; set; }
    public int RepeatIndex { get; set; }
}

public class PageItemModel
{
    public string PageId { get; set; }
    public int RepeatIndex { get; set; }
}

public class ConditionResult
{
    public bool MetCondition { get; set; }
    public string NextPageId { get; set; }
    public int RepeatIndex { get; set; }
}
