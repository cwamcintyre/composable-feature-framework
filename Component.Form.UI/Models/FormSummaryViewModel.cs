using System;
using Component.Form.Model;

namespace Component.Form.UI.Models;

public class FormSummaryViewModel
{
    public FormModel FormModel { get; set; }
    public List<PageSummaryItemViewModelBase> SummaryItems { get; set; } = new List<PageSummaryItemViewModelBase>();
}
