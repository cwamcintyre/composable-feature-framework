using System;
using Component.Form.Application.Helpers;
using Component.Form.Model;
using Component.Form.UI.ComponentHandler;
using Component.Form.UI.Models;
using Component.Form.UI.Services.Model;
using Newtonsoft.Json;

namespace Component.Form.UI.PageHandler.Default;

public class DefaultPageHandler : IPageHandler
{
    public const string ViewName = "ShowDefaultPage";

    private readonly ComponentHandlerFactory _componentHandlerFactory;

    public DefaultPageHandler(ComponentHandlerFactory componentHandlerFactory)
    {
        _componentHandlerFactory = componentHandlerFactory;
    }

    public bool IsFor(string type)
    {
        return type == "default" || String.IsNullOrEmpty(type);
    }

    public static string GetSafeType()
    {
        return SafeJsonHelper.GetSafeType(typeof(PageBase));
    }
    
    public async Task<ShowResult> HandlePage(PageBase page, GetDataForPageResponseModel dataResponse, string extraData)
    {
        var formData = dataResponse.FormData;
        if (formData != null) 
        {
            foreach (var data in formData)
            {
                var component = page.Components.Find(c => c.Name == data.Key);
                if (component != null)
                {
                    var handler = _componentHandlerFactory.GetFor(component.Type);
                    component.Answer = handler.GetFromObject(data.Value);
                }
            }
        }

        return new ShowResult
        {
            PageModel = page,
            Errors = dataResponse.Errors,
            ViewName = ViewName,
            PreviousPage = dataResponse.PreviousPage,
            PreviousExtraData = dataResponse.PreviousExtraData
        };
    }

    public async Task<Dictionary<string, string>> GetSubmittedPageData(PageBase page, Dictionary<string, string> data)
    {
        var formData = new Dictionary<string, string>();
        
        foreach (var component in page.Components.Where(c => c.IsQuestionType))
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

        return formData;
    }

    public async Task<PageSummaryItemViewModelBase> GetSummaryItem(PageBase page, Dictionary<string, object> formData)
    {
        var model = new DefaultSummaryItemViewModel();

        foreach (var component in page.Components.Where(c => c.IsQuestionType))
        {
            if (formData.TryGetValue(component.Name, out object? value))
            {
                var handler = _componentHandlerFactory.GetFor(component.Type);
                component.Answer = handler.GetFromObject(value);
            }
        }

        model.Components = page.Components;
        model.PartialName = "SummaryComponents/_DefaultSummaryItem";
        return model;
    }
}
