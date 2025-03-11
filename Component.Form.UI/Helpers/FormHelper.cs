using System;
using Component.Form.Model;
using Component.Form.Model.ComponentHandler;
using Newtonsoft.Json;

namespace Component.Form.UI.Helpers;

public class FormHelper
{
    private readonly ComponentHandlerFactory _componentHandlerFactory;

    public FormHelper(ComponentHandlerFactory componentHandlerFactory)
    {
        _componentHandlerFactory = componentHandlerFactory;
    }

    public Dictionary<string, string> GetSubmittedPageData(Page page, Dictionary<string, string> data)
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

        if (page.Repeating)
        {
            var repeatingFormData = new Dictionary<string, string>();
            var repeatModel = new RepeatingModel
            {
                FormData = formData,
                RepeatIndex = Convert.ToInt32(data["repeatIndex"])
            };
            repeatingFormData.Add(page.RepeatKey, JsonConvert.SerializeObject(repeatModel));
            return repeatingFormData;
        }
        else 
        {
            return formData;
        }
    }
}
