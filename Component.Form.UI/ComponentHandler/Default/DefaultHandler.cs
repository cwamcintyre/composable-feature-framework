using Component.Form.Application.Helpers;
using Component.Form.Model;

namespace Component.Form.UI.ComponentHandler.Default;

public class DefaultHandler : IComponentHandler
{
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
    
    public bool IsFor(string type)
    {
        return String.IsNullOrEmpty(type) || 
            type.Equals("text") ||
            type.Equals("select") ||
            type.Equals("multilineText") ||
            type.Equals("radio") ||
            type.Equals("checkbox") ||
            type.Equals("yesno") ||
            type.Equals("email") ||
            type.Equals("phonenumber") ||
            type.Equals("fileupload");    
    }

    public string GetPartialName(string type)
    {
        switch (type)
        {
            case "select":
                return "FormComponents/_Select";
            case "multilineText":
                return "FormComponents/_MultilineText";
            case "radio":
                return "FormComponents/_Radios";
            case "checkbox":
                return "FormComponents/_Checkboxes";
            case "yesno":
                return "FormComponents/_YesNo";
            case "email":
                return "FormComponents/_EmailAddress";
            case "phonenumber":
                return "FormComponents/_PhoneNumber";
            case "fileupload":
                return "FormComponents/_FileUpload";
            default:
                return "FormComponents/_TextInput";
        }
    }

    public string GetDataType()
    {
        return SafeJsonHelper.GetSafeType(typeof(string));
    }
}
