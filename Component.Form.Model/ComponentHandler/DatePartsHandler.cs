using System;
using Component.Form.Model.ComponentModel;
using Newtonsoft.Json.Linq;

namespace Component.Form.Model.ComponentHandler;

public class DatePartsHandler : IComponentHandler
{
    public const string ERR_DAY_REQUIRED = "Day is required";
    public const string ERR_MONTH_REQUIRED = "Month is required";
    public const string ERR_YEAR_REQUIRED = "Year is required";

    public const string ERR_DAY_OUT_OF_BOUNDS = "Day is between 1 and 31";
    public const string ERR_MONTH_OUT_OF_BOUNDS = "Month is between 1 and 12";
    public const string ERR_YEAR_OUT_OF_BOUNDS = "Year is between 1900 and 2100";

    public virtual object Get(string name, Dictionary<string, string> data)
    {
        var model = new DatePartsModel();

        if (data.TryGetValue($"{name}-day", out var day))
        {
            model.Day = int.TryParse(day, out var dayValue) ? dayValue : 0;
        }

        if (data.TryGetValue($"{name}-month", out var month))
        {
            model.Month = int.TryParse(month, out var monthValue) ? monthValue : 0;
        }

        if (data.TryGetValue($"{name}-year", out var year))
        {
            model.Year = int.TryParse(year, out var yearValue) ? yearValue : 0;;
        }

        return model;
    }

    public string GetDataType()
    {
        return ComponentHandlerFactory.GetDataType(typeof(DatePartsModel));
    }

    public object GetFromObject(object data)
    {
        if (data is JObject value)
        {
            return value.ToObject<DatePartsModel>();
        }

        throw new ArgumentException("DateParts data is not a JObject");
    }

    public bool IsFor(string type)
    {
        return type.Equals("dateparts", StringComparison.OrdinalIgnoreCase);
    }

    public async Task<List<string>> Validate(string name, object data, List<ValidationRule> validationExpressions, bool repeating = false, string repeatKey = "", int repeatIndex = 0)
    {
        var prefix = repeating ? $"Data.{repeatKey}[{repeatIndex}]" : $"Data";

        var validationRules = new List<ValidationRule>
        {
            new ValidationRule
            {
                Expression = $"{prefix}.{name} != null",
                ErrorMessage = "Date is required"
            },
            new ValidationRule
            {
                Expression = $"{prefix}.{name}.Day != null && {prefix}.{name}.Day > 0 && {prefix}.{name}.Day < 32",
                ErrorMessage = ERR_DAY_OUT_OF_BOUNDS
            },
            new ValidationRule
            {
                Expression = $"{prefix}.{name}.Month != null && {prefix}.{name}.Month > 0 && {prefix}.{name}.Month < 13",
                ErrorMessage = ERR_MONTH_OUT_OF_BOUNDS
            },
            new ValidationRule
            {
                Expression = $"{prefix}.{name}.Year != null && {prefix}.{name}.Year > 1899 && {prefix}.{name}.Year < 2101",
                ErrorMessage = ERR_YEAR_OUT_OF_BOUNDS
            }
        };

        if (validationExpressions != null) 
        {
            validationExpressions.AddRange(validationRules);
        }
        else validationExpressions = validationRules;

        return await ExpressionHelper.Validate(data, validationExpressions, repeatIndex);
    }
}
