using System;

namespace Component.Form.UI.Presenters;

public interface IRepeatingPagePresenter
{
    Task<RepeatingPageActionResult> HandleRemoveAction();
    Task<RepeatingPageActionResult> HandleAddAction();
}

public class RepeatingPagePresenter : IRepeatingPagePresenter
{
    public const string ShowPageAction = "ShowChangePage";
    public const string ShowSummaryAction = "Summary";

    public const string FormController = "Form";

    public async Task<RepeatingPageActionResult> HandleAddAction()
    {
        return new RepeatingPageActionResult()
        {
            Action = ShowPageAction,
            Controller = FormController
        };
    }

    public async Task<RepeatingPageActionResult> HandleRemoveAction()
    {
        return new RepeatingPageActionResult()
        {
            Action = ShowSummaryAction,
            Controller = FormController
        };
    }
}

public class RepeatingPageActionResult
{
    public string Action { get; set; }
    public string Controller { get; set; }
}
