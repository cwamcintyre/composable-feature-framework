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
        public required string PageType { get; set; }
        public required string Title { get; set; }
        public required List<Component> Components { get; set; }
        public List<Condition> Conditions { get; set; }
        public Dictionary<string, string> Data { get; set; }
        public required string NextPageId { get; set; }

        public bool Repeating { get; set; }
        public bool RepeatStart { get; set; }
        public bool RepeatEnd { get; set; }
        public string RepeatKey { get; set; }
        public int RepeatIndex { get; set; }
        public string SummaryKey { get; set; }  
    }
}
