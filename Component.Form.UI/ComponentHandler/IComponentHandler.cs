using System;
using Component.Form.Model;
using Newtonsoft.Json.Linq;

namespace Component.Form.UI.ComponentHandler;

public interface IComponentHandler
{
    bool IsFor(string type);
    object Get(string name, Dictionary<string,string> data);
    object GetFromObject(object data);
    string GetPartialName(string type);
}
