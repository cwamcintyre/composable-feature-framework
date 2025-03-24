using System;
using Component.Form.Application.Helpers;
using Component.Form.Model;
using Component.Form.Model.PageHandler;
using Component.Form.UI.ComponentHandler;
using Component.Form.UI.Models;
using Component.Form.UI.Services.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Component.Form.UI.PageHandler.InlineRepeatingPageHandler;

public class InlineRepeatingPageHandler : IPageHandler
{
    public const string ViewName = "ShowInlineRepeatingPage";

    private readonly ComponentHandlerFactory _componentHandlerFactory;
    private readonly SafeJsonHelper _safeJsonHelper;

    public InlineRepeatingPageHandler(ComponentHandlerFactory componentHandlerFactory, SafeJsonHelper safeJsonHelper)
    {
        _componentHandlerFactory = componentHandlerFactory;
        _safeJsonHelper = safeJsonHelper;
    }    
    
    public bool IsFor(string type)
    {
        return !String.IsNullOrEmpty(type) && type == "inlineRepeating";
    }

    public static string GetSafeType()
    {
        return SafeJsonHelper.GetSafeType(typeof(InlineRepeatingPageSection));
    }

    public async Task<ShowResult> HandlePage(PageBase page, GetDataForPageResponseModel dataModel, string extraData)
    {
        var repeatingData = (InlineRepeatingData)extraData;

        InlineRepeatingPageSection section = (InlineRepeatingPageSection)page;

        var formData = dataModel.FormData;

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

        if (formData.ContainsKey(section.RepeatKey))
        {
            var repeatData = _safeJsonHelper.SafeDeserializeObject<Dictionary<string,string>>(formData[section.RepeatKey].ToString());

            foreach (var data in repeatData)
            {
                var component = repeatPage.Components.Find(c => c.Name == data.Key);
                if (component != null)
                {
                    var handler = _componentHandlerFactory.GetFor(component.Type);
                    component.Answer = handler.GetFromObject(data.Value);
                }
            }
        }

        repeatPage.RepeatIndex = repeatingData.RepeatIndex;
        repeatPage.SectionId = section.PageId;

        return new ShowResult
        {
            PageModel = repeatPage,
            Errors = dataModel.Errors,
            ViewName = ViewName,
            PreviousPage = dataModel.PreviousPage,
            PreviousExtraData = dataModel.PreviousExtraData
        };
    }
    
    public async Task<Dictionary<string, string>> GetSubmittedPageData(PageBase page, Dictionary<string, string> data)
    {
        var formData = new Dictionary<string, string>();
        var repeatIndex = Convert.ToInt32(data["RepeatIndex"]);
        var repeatPageId = data["RepeatPageId"];
        var extraData = data["ExtraData"];

        InlineRepeatingPageSection section = (InlineRepeatingPageSection)page;

        var repeatPage = section.RepeatingPages.Find(p => p.PageId == repeatPageId);

        foreach (var component in repeatPage.Components.Where(c => c.IsQuestionType))
        {
            var handler = _componentHandlerFactory.GetFor(component.Type);
            var value = handler.Get(component.Name, data);
            
            if (value.GetType() == typeof(string))
            {
                formData.Add(component.Name, value.ToString());
            }
            else
            {
                formData.Add(component.Name, JsonConvert.SerializeObject(value));
            }
        }

        var repeatModel = new RepeatingModel
        {
            RepeatIndex = repeatIndex,
            FormData = formData
        };

        return new Dictionary<string, string>
        {
            { section.RepeatKey, JsonConvert.SerializeObject(repeatModel) },
            { "RepeatIndex", repeatIndex.ToString() },
            { "RepeatPageId", repeatPageId },
            { "ExtraData", extraData }
        };
    }

    public async Task<PageSummaryItemViewModelBase> GetSummaryItem(PageBase page, Dictionary<string, object> formData)
    {
        var section = (InlineRepeatingPageSection)page;

        var repeatDataList = _safeJsonHelper.SafeDeserializeObject<List<Dictionary<string, object>>>(formData[section.RepeatKey].ToString());

        var repeatingData = new List<List<ComponentSummaryItem>>();

        foreach (var data in repeatDataList)
        {
            var components = new List<ComponentSummaryItem>();
            
            foreach (var repeatingPage in section.RepeatingPages)
            {
                foreach (var component in repeatingPage.Components.Where(c => c.IsQuestionType))
                {
                    var componentClone = component.Clone();
                    if (data.TryGetValue(componentClone.Name, out object? value))
                    {
                        var handler = _componentHandlerFactory.GetFor(componentClone.Type);
                        componentClone.Answer = handler.GetFromObject(value);
                    }
                    components.Add(new ComponentSummaryItem
                    {
                        PageId = repeatingPage.PageId,
                        Component = componentClone,
                        ShowChangeLink = !repeatingPage.RepeatEnd
                    });
                }                
            }

            repeatingData.Add(components);
        }

        var summary = new InlineRepeatingSummaryItemViewModel
        {
            PartialName = "SummaryComponents/_InlineRepeatingSummaryItem",
            RepeatingData = repeatingData,
            SummaryLabel = section.SummaryLabel,
            PageId = section.PageId
        };

        return summary;
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
            throw new ArgumentException($"Invalid InlineRepeatingData format - expected repeatIndex-pageId, got {data}");
        }

        return new InlineRepeatingData
        {
            PageId = split[1],
            RepeatIndex = Convert.ToInt32(split[0])
        };
    }
}
