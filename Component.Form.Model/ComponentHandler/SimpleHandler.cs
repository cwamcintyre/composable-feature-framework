using System;
using System.ComponentModel;
using Component.Form.Model.ComponentHandler;
using Component.Form.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Component.Form.Model.ComponentHandler;

/// <summary>
/// This class is a simple handler for components. THIS MUST NOT BE INJECTED INTO THE DI CONTAINER.
/// </summary>
public class SimpleHandler : IComponentHandler
{
    private static readonly Lazy<SimpleHandler> instance = new Lazy<SimpleHandler>(() => new SimpleHandler());

    public static SimpleHandler Instance => instance.Value;

    private SimpleHandler() {} // prevent DI injection

    public object Get(string name, Dictionary<string, string> data)
    {
        if (data.ContainsKey(name))
        {
            return data[name];
        }

        return "";
    }

    public object GetFromObject(object data)
    {
        if (data is string value)
        {
            return value;
        }

        throw new ArgumentException("string data is not a string");
    }

    public string GetDataType()
    {
        return ComponentHandlerFactory.GetDataType(typeof(string));
    }

    public bool IsFor(string type)
    {
        throw new NotImplementedException();       
    }

    public async Task<List<string>> Validate(string name, dynamic data, List<ValidationRule> validationRules)
    {
        if (validationRules == null || validationRules.Count == 0)
        {
            return new List<string>();
        }
        
        return await ExpressionHelper.Validate(data, validationRules);
    }
}
