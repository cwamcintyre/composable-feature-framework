using System.Dynamic;
using Component.Form.Application.ComponentHandler;
using Component.Form.Application.PageHandler;
using Component.Form.Model;
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

    // considering that the user has the back link and the back button (and we cannot effectively test for the back button), we have to calculate the previous page
    // based on first principles every time a page is loaded.
    public static async Task<PreviousPageModel> CalculatePreviousPage(
        PageHandlerFactory pageHandlerFactory,
        FormModel formModel, 
        string currentPageId, 
        IDictionary<string, object> data, 
        string extraData)
    {
        // calculate forward journey up until now...
        var pages = new Stack<PageItemModel>();

        if (!String.IsNullOrEmpty(extraData))
        {
            currentPageId = $"{currentPageId}/{extraData}";
        }

        await CalculatePreviousPageRecursive(
            pageHandlerFactory, 
            formModel, 
            formModel.Pages[0], 
            currentPageId, 
            data, 
            pages,
            extraData);

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
            RepeatIndex = pages.Peek().RepeatIndex,
            ExtraData = pages.Peek().ExtraData
        };
    }

    private static async Task CalculatePreviousPageRecursive(
        PageHandlerFactory pageHandlerFactory,
        FormModel formModel,
        PageBase page, 
        string currentPageId, 
        IDictionary<string, object> data, 
        Stack<PageItemModel> pages,
        string extraData,
        string nextExtraData = "")
    {
        var checkPageId = page.PageId;
        if (!String.IsNullOrEmpty(nextExtraData))
        {
            checkPageId = $"{checkPageId}/{nextExtraData}";
        }

        if (checkPageId == currentPageId)
        {
            return;
        }   

        pages.Push(new PageItemModel { PageId = page.PageId, ExtraData = nextExtraData });
                
        var pageHandler = pageHandlerFactory.GetFor(page.PageType);

        if (pageHandler == null)
        {
            throw new ArgumentException($"Page handler not found for page type {page.PageType}");
        }

        var nextPageIdResult = await pageHandler.GetNextPageId(page, data, nextExtraData);    
        var nextPage = formModel.Pages.FirstOrDefault(p => p.PageId == nextPageIdResult.NextPageId);
        if (nextPage == null)
        {
            throw new ArgumentException($"Next page {nextPageIdResult.NextPageId} not found");
        }

        await CalculatePreviousPageRecursive(pageHandlerFactory, formModel, nextPage, currentPageId, data, pages, extraData, nextPageIdResult.ExtraData);        
    }
}

public class PreviousPageModel 
{
    public string PageId { get; set; }
    public int RepeatIndex { get; set; }
    public string ExtraData { get; set; }
}

public class PageItemModel
{
    public string PageId { get; set; }
    public int RepeatIndex { get; set; }
    public string ExtraData { get; set; }
}

public class ConditionResult
{
    public bool MetCondition { get; set; }
    public string NextPageId { get; set; }
    public string ExtraData { get; set; }
    public int RepeatIndex { get; set; }
}
