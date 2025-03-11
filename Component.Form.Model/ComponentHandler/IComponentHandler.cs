using System;
using Component.Form.Model;
using Newtonsoft.Json.Linq;

namespace Component.Form.Model.ComponentHandler;

public interface IComponentHandler
{
    bool IsFor(string type);
    object Get(string name, Dictionary<string,string> data);
    object GetFromObject(object data);
    string GetDataType();
    Task<List<string>> Validate(string name, object data, List<ValidationRule> validationExpressions, bool repeating, string repeatKey);
}
