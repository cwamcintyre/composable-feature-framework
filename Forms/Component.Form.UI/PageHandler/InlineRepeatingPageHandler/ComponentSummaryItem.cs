using System;

namespace Component.Form.UI.PageHandler.InlineRepeatingPageHandler;

public class ComponentSummaryItem
{
    public string PageId { get; set; }
    public Model.Component Component { get; set; }
    public bool ShowChangeLink { get; set; } = true;
}
