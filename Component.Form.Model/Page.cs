using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component.Form.Model
{
    public class Page
    {
        public required string PageId { get; set; }
        public required string Title { get; set; }
        public required List<Question> Questions { get; set; }
        public required Condition Conditions { get; set; }
    }
}
