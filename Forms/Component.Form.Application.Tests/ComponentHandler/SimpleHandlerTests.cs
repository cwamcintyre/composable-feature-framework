using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Component.Form.Model.ComponentHandler;
using Component.Form.Application.ComponentHandler.Default;
using Component.Form.Model;

namespace Component.Form.Application.ComponentHandler.Tests;

public class SimpleHandlerTests
{
    private readonly DefaultHandler _handler;
    
    public SimpleHandlerTests()
    {
        _handler = new DefaultHandler();
    }

    [Fact]
    public void Get_ReturnsValue_WhenKeyExists()
    {
        var data = new Dictionary<string, string> { { "key", "value" } };
        var result = _handler.Get("key", data);
        Assert.Equal("value", result);
    }

    [Fact]
    public void Get_ReturnsEmptyString_WhenKeyDoesNotExist()
    {
        var data = new Dictionary<string, string>();
        var result = _handler.Get("key", data);
        Assert.Equal("", result);
    }

    [Fact]
    public void GetFromObject_ReturnsString_WhenDataIsString()
    {
        var result = _handler.GetFromObject("value");
        Assert.Equal("value", result);
    }

    [Fact]
    public void GetDataType_ReturnsStringType()
    {
        var result = _handler.GetDataType();
        Assert.Equal("System.String, System.Private.CoreLib", result);
    }

    [Fact]
    public async Task Validate_ReturnsEmptyList_WhenNoValidationRules()
    {
        var result = await _handler.Validate("name", new object(), new List<ValidationRule>());
        Assert.Empty(result);
    }

    // Add more tests for Validate method as needed
}
