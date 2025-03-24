using System.Diagnostics.CodeAnalysis;
using Component.Form.Application.ComponentHandler.PhoneNumber;
using Component.Form.Model;

namespace Component.Form.Application.ComponentHandler.Tests;

[ExcludeFromCodeCoverage]
public class PhoneNumberHandlerTests
{
    private readonly PhoneNumberHandler _handler;

    public PhoneNumberHandlerTests()
    {
        _handler = new PhoneNumberHandler();
    }

    [Fact]
    public void Get_ShouldReturnPhoneNumber_WhenNameExistsInData()
    {
        var data = new Dictionary<string, string> { { "phone", "1234567890" } };
        var result = _handler.Get("phone", data);
        Assert.Equal("1234567890", result);
    }

    [Fact]
    public void GetFromObject_ShouldReturnPhoneNumber_WhenDataIsString()
    {
        var result = _handler.GetFromObject("1234567890");
        Assert.Equal("1234567890", result);
    }

    [Fact]
    public void GetDataType_ShouldReturnStringType()
    {
        var result = _handler.GetDataType();
        Assert.Equal("System.String, System.Private.CoreLib", result);
    }

    [Fact]
    public void IsFor_ShouldReturnTrue_WhenTypeIsPhoneNumber()
    {
        var result = _handler.IsFor("phonenumber");
        Assert.True(result);
    }

    [Fact]
    public void IsFor_ShouldReturnFalse_WhenTypeIsNotPhoneNumber()
    {
        var result = _handler.IsFor("email");
        Assert.False(result);
    }

    [Fact]
    public async Task Validate_ShouldReturnErrors_WhenPhoneNumberIsInvalid()
    {
        var data = new Dictionary<string, object> { { "phone", "invalid" } };
        var validationRules = new List<ValidationRule>();
        var result = await _handler.Validate("phone", data, validationRules);
        Assert.Contains("Enter a UK phone number", result);
    }

    [Fact]
    public async Task Validate_ShouldReturnNoErrors_WhenPhoneNumberIsValid()
    {
        var data = new Dictionary<string, object> { { "phone", "+447911123456" } };
        var validationRules = new List<ValidationRule>();
        var result = await _handler.Validate("phone", data, validationRules);
        Assert.DoesNotContain("Enter a UK phone number", result);
    }
}
