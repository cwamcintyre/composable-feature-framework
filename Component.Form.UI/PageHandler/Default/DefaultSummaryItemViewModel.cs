using Component.Form.UI.Models;

namespace Component.Form.UI.PageHandler.Default;

public class DefaultSummaryItemViewModel : PageSummaryItemViewModelBase
{
    public List<Model.Component> Components { get; set; } = new List<Model.Component>();
}
