using System;
using System.Collections.Generic;
using Component.Form.Application.ComponentHandler;
using Component.Form.Application.PageHandler.Default;
using Component.Form.Application.Shared.Infrastructure;
using Component.Form.Model;
using Moq;

namespace Component.Form.Application.Tests.Doubles.PageHandler;

public class DefaultPageHandlerTestBuilder
{
        private readonly Mock<IFormStore> _formStoreMock;
        private readonly Mock<IFormDataStore> _formDataStoreMock;
        private readonly Mock<IComponentHandlerFactory> _componentHandlerFactoryMock;

        public DefaultPageHandlerTestBuilder()
        {
            _formStoreMock = new Mock<IFormStore>();
            _formDataStoreMock = new Mock<IFormDataStore>();
            _componentHandlerFactoryMock = new Mock<IComponentHandlerFactory>();
        }

        public DefaultPageHandlerTestBuilder WithComponentHandlerValidation(string inputName, List<string> validationResult, string dataType = "System.String, System.Private.CoreLib")
        {
            var handlerMock = new Mock<IComponentHandler>();
            handlerMock.Setup(h => 
                h.Validate(
                    inputName, 
                    It.IsAny<IDictionary<string, object>>(), 
                    It.IsAny<List<ValidationRule>>(),
                    It.IsAny<bool>(),
                    It.IsAny<string>(),
                    It.IsAny<int>()
                ))
                .Returns(Task.FromResult(validationResult));
            handlerMock.Setup(h => h.GetDataType()).Returns(dataType);

        _componentHandlerFactoryMock.Setup(f => f.GetFor(It.IsAny<string>())).Returns(handlerMock.Object);

            return this;
        }

        public DefaultPageHandler Build()
        {
            return new DefaultPageHandler(
                _formStoreMock.Object,
                _formDataStoreMock.Object,
                _componentHandlerFactoryMock.Object
            );
        }
}

public class ComponentTestBuilder
{
    private readonly Model.Component _component;

    public ComponentTestBuilder()
    {
        _component = new Model.Component
        {
            QuestionId = "default-question-id",
            Type = "default-type",
            Label = "default-label",
            Name = "default-name",
            Required = false,
            LabelIsPageTitle = false,
            Options = new Dictionary<string, string>(),
            FileOptions = new Model.FileOptions
            {
                MaxSizeMB = 10,
                AcceptedFormats = new List<string> { ".jpg", ".png", ".pdf" },
                UploadEndpoint = "https://example.com/upload"
            },
            Optional = false,
            ValidationRules = new List<ValidationRule>()
        };
    }

    public ComponentTestBuilder WithQuestionId(string questionId)
    {
        _component.QuestionId = questionId;
        return this;
    }

    public ComponentTestBuilder WithType(string type)
    {
        _component.Type = type;
        return this;
    }

    public ComponentTestBuilder WithLabel(string label)
    {
        _component.Label = label;
        return this;
    }

    public ComponentTestBuilder WithName(string name)
    {
        _component.Name = name;
        return this;
    }

    public ComponentTestBuilder WithHint(string? hint)
    {
        _component.Hint = hint;
        return this;
    }

    public ComponentTestBuilder WithRequired(bool required)
    {
        _component.Required = required;
        return this;
    }

    public ComponentTestBuilder WithLabelIsPageTitle(bool labelIsPageTitle)
    {
        _component.LabelIsPageTitle = labelIsPageTitle;
        return this;
    }

    public ComponentTestBuilder WithAnswer(object? answer)
    {
        _component.Answer = answer;
        return this;
    }

    public ComponentTestBuilder WithOptions(Dictionary<string, string> options)
    {
        _component.Options = options;
        return this;
    }

    public ComponentTestBuilder WithFileOptions(Model.FileOptions fileOptions)
    {
        _component.FileOptions = fileOptions;
        return this;
    }

    public ComponentTestBuilder WithContent(string? content)
    {
        _component.Content = content;
        return this;
    }

    public ComponentTestBuilder WithOptional(bool optional)
    {
        _component.Optional = optional;
        return this;
    }

    public ComponentTestBuilder WithValidationRules(List<ValidationRule> validationRules)
    {
        _component.ValidationRules = validationRules;
        return this;
    }

    public ComponentTestBuilder WithValidationRule(ValidationRule validationRule)
    {
        _component.ValidationRules.Add(validationRule);
        return this;
    }
    
    public ComponentTestBuilder WithValidationRule(string name, string errorMessage, string type, string expression, string? errorMessageKey = null)
    {
        var validationRule = new ValidationRule
        {
            ErrorMessage = errorMessage,
            Expression = expression,
        };
        _component.ValidationRules.Add(validationRule);
        return this;
    }

    public Model.Component Build()
    {
        return _component;
    }
}
