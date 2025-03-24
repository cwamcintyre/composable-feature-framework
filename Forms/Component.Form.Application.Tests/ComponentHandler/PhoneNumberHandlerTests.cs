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

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("123456")]
    [InlineData("1234567890")]
    [InlineData("12345678901234567890")]
    [InlineData("+1234567890")]
    public async Task Validate_ShouldReturnErrors_WhenPhoneNumberIsInvalid(string phoneNumber)
    {
        var data = new Dictionary<string, object> { { "phone", phoneNumber } };
        var validationRules = new List<ValidationRule>();
        var result = await _handler.Validate("phone", data, validationRules);
        Assert.Contains("Enter a UK phone number", result);
    }

    [Theory]
    [InlineData("+447911123456")]
    [InlineData("07911123456")]
    public async Task Validate_ShouldReturnNoErrors_WhenPhoneNumberIsValid(string phoneNumber)
    {
        var data = new Dictionary<string, object> { { "phone", phoneNumber } };
        var validationRules = new List<ValidationRule>();
        var result = await _handler.Validate("phone", data, validationRules);
        Assert.DoesNotContain("Enter a UK phone number", result);
    }
}
