using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component.Form.Model
{
    public class Question
    {
        public required string QuestionId { get; set; }
        public required string Type { get; set; }
        public required string Label { get; set; }
        public required bool Required { get; set; }
        public Dictionary<string, string> Options { get; set; }
        public FileOptions FileOptions { get; set; }

        public Dictionary<string, string> Branching { get; set; }
    }
}
