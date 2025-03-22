using Component.Form.Model;
using Component.Form.UI.ComponentHandler;
using Component.Form.UI.Models;
using Component.Form.UI.PageHandler;
using Component.Form.UI.Services.Model;
using Newtonsoft.Json;

public interface IFormPresenter
{
    Task<SubmitResult> HandleSubmit(FormModel formModel, ProcessFormResponseModel response);
    Task<SubmitResult> HandleChangeSubmit(FormModel formModel, ProcessFormResponseModel response);
    Task<StopResult> HandleStop(string pageId, FormModel formModel);    
    Task<SummaryResult> HandleSummary(FormModel formModel, GetDataResponseModel response);
    Task<ConfirmationResult> HandleConfirmation(string applicationId);
}

public class FormPresenter : IFormPresenter
{
    public virtual string IndexAction { get; set; } = "ShowPage";
    public virtual string ChangeAction { get; set; } = "ShowChangePage";
    public virtual string StopAction { get; set; } = "Stop";

    public virtual string SummaryAction { get; set; } = "Summary";

    public virtual string ConfirmationAction { get; set; } = "Confirmation";   

    private readonly ComponentHandlerFactory _componentHandlerFactory;
    private readonly PageHandlerFactory _pageHandlerFactory;

    public FormPresenter(ComponentHandlerFactory componentHandlerFactory, PageHandlerFactory pageHandlerFactory)
    {
        _componentHandlerFactory = componentHandlerFactory;
        _pageHandlerFactory = pageHandlerFactory;
    }

    public virtual async Task<SubmitResult> HandleSubmit(FormModel formModel, ProcessFormResponseModel response)
    {
        var nextPageId = response.NextPageId;
        var extraData = response.ExtraData;

        var currentPage = formModel.Pages.Find(p => p.PageId == nextPageId);
        if (currentPage == null) return null;

        if (!String.IsNullOrEmpty(currentPage.PageType) && currentPage.PageType.Equals("summary"))
        {
            return new SubmitResult()
            {
                NextAction = SummaryAction
            };
        }

        if (!String.IsNullOrEmpty(currentPage.PageType) && currentPage.PageType.Equals("stop"))
        {
            return new SubmitResult()
            {
                NextAction = StopAction,
                NextPage = response.NextPageId
            };
        }

        return new SubmitResult
        {
            NextAction = IndexAction,
            NextPage = nextPageId,
            ExtraData = extraData
        };
    }

    public virtual async Task<SubmitResult> HandleChangeSubmit(FormModel formModel, ProcessFormResponseModel response) 
    {
        var result = await HandleSubmit(formModel, response);

        if (result.NextAction.Equals(IndexAction))
        {
            result.NextAction = ChangeAction;
        }

        return result;
    }

    public virtual async Task<SummaryResult> HandleSummary(FormModel formModel, GetDataResponseModel response)
    {
        var formData = new Dictionary<string, object>();
        
        if (!String.IsNullOrEmpty(response.FormData.Data))
        {
            formData = JsonConvert.DeserializeObject<Dictionary<string, object>>(response.FormData.Data);
        }

        var formSummaryViewModel = new FormSummaryViewModel
        {
            FormModel = formModel
        };

        foreach (var page in formModel.Pages) 
        {
            // REFACTOR SO THESE ITEMS HAVE A PAGE HANDLER?
            if (page.PageType == "summary") continue;
            if (page.PageType == "stop") continue;

            var pageHandler = _pageHandlerFactory.GetFor(page.PageType);

            var summaryItem = await pageHandler.GetSummaryItem(page, formData);
            if (summaryItem != null)
            {
                summaryItem.PageId = page.PageId;
                formSummaryViewModel.SummaryItems.Add(summaryItem);
            }
        }

        return new SummaryResult
        {
            SummaryModel = formSummaryViewModel,
            ViewName = SummaryAction
        };
    }

    public virtual async Task<StopResult> HandleStop(string pageId, FormModel formModel)
    {
        var currentPage = formModel.Pages.Find(p => p.PageId == pageId);
        if (currentPage == null) return null;

        return new StopResult
        {
            ViewName = StopAction,
            PageModel = currentPage
        };
    }

    public virtual async Task<ConfirmationResult> HandleConfirmation(string applicationId)
    {
        return new ConfirmationResult
        {
            ApplicationId = applicationId,
            ViewName = ConfirmationAction
        };
    }
}

public class SubmitResult
{
    public string NextAction { get; set; }
    public string NextPage {get; set; }
    public string ExtraData { get; set; }
}

public class SummaryResult
{
    public FormSummaryViewModel SummaryModel { get; set; }
    public string ViewName { get; set; }
}

public class StopResult
{
    public PageBase PageModel { get; set; }
    public string ViewName { get; set; }
}

public class ConfirmationResult
{
    public string ApplicationId { get; set; }
    public string ViewName { get; set; }
}