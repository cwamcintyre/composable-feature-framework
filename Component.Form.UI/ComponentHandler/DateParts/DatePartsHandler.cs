using Component.Form.UI.ComponentHandler;
using Component.Form.Model.ComponentHandler;
using Newtonsoft.Json.Linq;
using Component.Form.Application.Helpers;

namespace Component.Form.Model.ComponentHandler.DateParts;

public class DatePartsHandler : IComponentHandler
{
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

    public string GetPartialName(string type)
    {
        return "FormComponents/_DateParts";
    }

    public string GetDataType()
    {
        return SafeJsonHelper.GetSafeType(typeof(string));
    }
}
