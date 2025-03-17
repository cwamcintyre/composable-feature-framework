using Component.Form.Model;

namespace Component.Form.Model.PageHandler
{
    public class InlineRepeatingPageSection : PageBase
    {
        public string RepeatKey { get; set; }
        public string SummaryLabel { get; set; }
        public List<InlineRepeatingPage> RepeatingPages { get; set; }   
    }
}