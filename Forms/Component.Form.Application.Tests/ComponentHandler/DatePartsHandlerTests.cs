using Newtonsoft.Json.Linq;
using System.Dynamic;
using Component.Form.Application.ComponentHandler.DateParts;
using Component.Form.Model.ComponentHandler;
using Component.Form.Model;

namespace Component.Form.Application.ComponentHandler.Tests;

public class DatePartsHandlerTests
{
    private readonly DatePartsHandler _handler;

    public DatePartsHandlerTests()
    {
        _handler = new DatePartsHandler();
    }

    [Fact]
    public void Get_ShouldReturnDatePartsModel()
    {
        var data = new Dictionary<string, string>
        {
            { "date-day", "15" },
            { "date-month", "8" },
            { "date-year", "2023" }
        };

        var result = _handler.Get("date", data) as DatePartsModel;

        Assert.NotNull(result);
        Assert.Equal(15, result.Day);
        Assert.Equal(8, result.Month);
        Assert.Equal(2023, result.Year);
    }

    [Fact]
    public void GetFromObject_ShouldReturnDatePartsModel()
    {
        var jObject = JObject.Parse("{ 'Day': 15, 'Month': 8, 'Year': 2023 }");

        var result = _handler.GetFromObject(jObject) as DatePartsModel;

        Assert.NotNull(result);
        Assert.Equal(15, result.Day);
        Assert.Equal(8, result.Month);
        Assert.Equal(2023, result.Year);
    }

    [Fact]
    public void IsFor_ShouldReturnTrueForDatePartsType()
    {
        var result = _handler.IsFor("dateparts");

        Assert.True(result);
    }

    [Fact]
    public async Task Validate_ShouldReturnValidationErrors()
    {
        var datePartsData = new DatePartsModel { Day = 32, Month = 13, Year = 1800 };
        var validationRules = new List<ValidationRule>();

        dynamic data = new ExpandoObject();
        data.date = datePartsData;

        var result = await _handler.Validate("date", data, validationRules);

        Assert.Contains(DatePartsHandler.ERR_DAY_OUT_OF_BOUNDS, result);
        Assert.Contains(DatePartsHandler.ERR_MONTH_OUT_OF_BOUNDS, result);
        Assert.Contains(DatePartsHandler.ERR_YEAR_OUT_OF_BOUNDS, result);
    }

    [Fact]
    public async Task Validate_ShouldPassForValidData()
    {
        var datePartsData = new DatePartsModel { Day = 15, Month = 8, Year = 2023 };
        var validationRules = new List<ValidationRule>();

        dynamic data = new ExpandoObject();
        data.date = datePartsData;

        var result = await _handler.Validate("date", data, validationRules);

        Assert.Empty(result);
    }
}
