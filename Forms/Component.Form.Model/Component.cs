using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component.Form.Model
{
    public class Component
    {
        public required string QuestionId { get; set; }
        public required string Type { get; set; }
        public required string Label { get; set; }
        public required string Name { get; set; }
        public string? Hint { get; set; }
        public required bool Required { get; set; }
        public bool LabelIsPageTitle { get; set; }
        public object? Answer { get; set; }
        public Dictionary<string, string> Options { get; set; }
        public FileOptions FileOptions { get; set; }
        public string? Content { get; set; }
        public bool Optional { get; set; }
        public List<ValidationRule> ValidationRules { get; set; }
        public bool IsQuestionType { get { return Type != "html" && Type != "summary"; } }

        /// <summary>
        /// Creates a shallow copy of the current component.
        /// </summary>
        /// <returns>Component</returns>
        public Component Clone()
        {
            return new Component
            {
                QuestionId = this.QuestionId,
                Type = this.Type,
                Label = this.Label,
                Name = this.Name,
                Required = this.Required,
                LabelIsPageTitle = this.LabelIsPageTitle,
                Options = this.Options,
                FileOptions = this.FileOptions,
                Content = this.Content,
                Optional = this.Optional,
                ValidationRules = this.ValidationRules
            };
        }
    }
}
