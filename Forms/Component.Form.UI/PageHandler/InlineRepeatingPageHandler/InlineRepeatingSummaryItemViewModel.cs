using Component.Form.UI.Models;

namespace Component.Form.UI.PageHandler.InlineRepeatingPageHandler;

public class InlineRepeatingSummaryItemViewModel : PageSummaryItemViewModelBase
{
    public string SummaryLabel { get; set; }
    public List<List<ComponentSummaryItem>> RepeatingData { get; set; } = new List<List<ComponentSummaryItem>>();
}
