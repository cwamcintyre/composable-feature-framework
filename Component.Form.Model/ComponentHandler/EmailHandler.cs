using Component.Form.Model.ComponentModel;
using Newtonsoft.Json.Linq;
using System.Net.Mail;

namespace Component.Form.Model.ComponentHandler;

public class EmailHandler : IComponentHandler
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

        throw new ArgumentException("Email data is not a string");
    }
    
    public string GetDataType()
    {
        return ComponentHandlerFactory.GetDataType(typeof(string));
    }

    public bool IsFor(string type)
    {
        return type.Equals("email", StringComparison.OrdinalIgnoreCase);
    }

    public async Task<List<string>> Validate(string name, object data, List<ValidationRule> validationExpressions, bool repeating = false, string repeatKey = "", int repeatIndex = 0)
    {
        var errors = new List<string>();

        var email = ((IDictionary<string, object>)data)[name].ToString();        

        if (string.IsNullOrEmpty(email) || !IsValidEmail(email))
        {
            errors.Add($"Enter an email address in the correct format, like name@example.com");
        }

        if (validationExpressions == null || validationExpressions.Count == 0)
        {
            errors.AddRange(await ExpressionHelper.Validate(data, validationExpressions, repeatIndex));
        }

        return errors;
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new MailAddress(email);
            return addr.Address == email; // Ensures the email wasn't auto-corrected
        }
        catch
        {
            return false; // Invalid email format
        }
    }
}
