using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component.Form.Model
{

    public class Condition
    {
        public required string PreviousQuestionId { get; set; }
        public required string ExpectedAnswer { get; set; }
    }
}
