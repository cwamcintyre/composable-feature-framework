using System.Diagnostics.CodeAnalysis;
using Component.Form.Application.ComponentHandler.Email;
using Component.Form.Model;

namespace Component.Form.Application.ComponentHandler.Tests;

[ExcludeFromCodeCoverage]
public class EmailHandlerTests
{
    private readonly EmailHandler _emailHandler;

    public EmailHandlerTests()
    {
        _emailHandler = new EmailHandler();
    }

    [Fact]
    public void GetDataType_ShouldReturnStringType()
    {
        var result = _emailHandler.GetDataType();
        Assert.Equal("System.String, System.Private.CoreLib", result);
    }

    [Fact]
    public void IsFor_ShouldReturnTrue_WhenTypeIsEmail()
    {
        var result = _emailHandler.IsFor("email");
        Assert.True(result);
    }

    [Fact]
    public void IsFor_ShouldReturnFalse_WhenTypeIsNotEmail()
    {
        var result = _emailHandler.IsFor("notemail");
        Assert.False(result);
    }

    [Fact]
    public async Task Validate_ShouldReturnErrors_WhenEmailIsInvalid()
    {
        var data = new Dictionary<string, object> { { "email", "invalidemail" } };
        var result = await _emailHandler.Validate("email", data, new List<ValidationRule>());
        Assert.Contains("Enter an email address in the correct format, like name@example.com", result);
    }

    [Fact]
    public async Task Validate_ShouldReturnNoErrors_WhenEmailIsValid()
    {
        var data = new Dictionary<string, object> { { "email", "test@example.com" } };
        var result = await _emailHandler.Validate("email", data, new List<ValidationRule>());
        Assert.Empty(result);
    }
}
