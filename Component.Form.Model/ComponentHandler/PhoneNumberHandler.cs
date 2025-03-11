using System;
using PhoneNumbers;

namespace Component.Form.Model.ComponentHandler;

public class PhoneNumberHandler: IComponentHandler
{
    public virtual object Get(string name, Dictionary<string, string> data)
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

        throw new ArgumentException("Phone number data is not a string");
    }
    
    public string GetDataType()
    {
        return ComponentHandlerFactory.GetDataType(typeof(string));
    }

    public bool IsFor(string type)
    {
        return type.Equals("phonenumber", StringComparison.OrdinalIgnoreCase);
    }

    public async Task<List<string>> Validate(string name, object data, List<ValidationRule> validationExpressions, bool repeating = false, string repeatKey = "")
    {
        var errors = new List<string>();

        var phoneNumber = ((IDictionary<string, object>)data)[name].ToString();        

        if (string.IsNullOrEmpty(phoneNumber) || !IsValidPhoneNumber(phoneNumber, "GB"))
        {
            errors.Add($"Enter a UK phone number");
        }

        if (validationExpressions == null || validationExpressions.Count == 0)
        {
            errors.AddRange(await ExpressionHelper.Validate(data, validationExpressions));
        }

        return errors;
    }

    private static bool IsValidPhoneNumber(string phone, string region)
    {
        try
        {
            var phoneUtil = PhoneNumberUtil.GetInstance();
            var parsedNumber = phoneUtil.Parse(phone, region);
            return phoneUtil.IsValidNumber(parsedNumber);
        }
        catch (NumberParseException)
        {
            return false;
        }
    }
}