using Component.Form.Application.Helpers;
using Component.Form.Model.ComponentHandler;
using Newtonsoft.Json.Linq;

namespace Component.Form.UI.ComponentHandler;

public class UkAddressHandler : IComponentHandler
{
    public const string ERR_LINE_1_REQUIRED = "Address Line 1 is required";
    public const string ERR_POSTCODE_REQUIRED = "Postcode is required";

    public virtual object Get(string name, Dictionary<string, string> data)
    {
        var model = new UkAddressModel();
        
        // COPILOT: refactor to test whether the key exists in the dictionary. if a key does not exist, add it to an errors list. if any key does not exist at the end, throw an argument exception that lists the missing keys.
        // refactor to remove duplicate code
        model.AddressLine1 = data[$"{name}-addressLine1"];
        model.AddressLine2 = data[$"{name}-addressLine2"];
        model.Town = data[$"{name}-addressTown"];
        model.County = data[$"{name}-addressCounty"];
        model.Postcode = data[$"{name}-addressPostcode"];

        return model;
    }

    public object GetFromObject(object data)
    {
        if (data is JObject value)
        {
            return value.ToObject<UkAddressModel>();
        }

        throw new ArgumentException("UkAddress data is not a JObject");
    }
    
    public bool IsFor(string type)
    {
        return type.Equals("ukaddress", StringComparison.CurrentCultureIgnoreCase);        
    }

    public string GetPartialName(string type)
    {
        return "FormComponents/_UkAddress";
    }

    public string GetDataType()
    {
        return SafeJsonHelper.GetSafeType(typeof(string));
    }
}