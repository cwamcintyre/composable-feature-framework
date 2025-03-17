using Component.Form.Model;
using Component.Form.UI.ComponentHandler;
using Component.Form.UI.PageHandler;
using Component.Form.UI.Services.Model;
using Newtonsoft.Json;

public interface IFormPresenter
{
    Task<SubmitResult> HandleSubmit(FormModel formModel, ProcessFormResponseModel response);
    Task<StopResult> HandleStop(string pageId, FormModel formModel);    
    Task<SummaryResult> HandleSummary(FormModel formModel, GetDataResponseModel response);
    Task<ConfirmationResult> HandleConfirmation(string applicationId);
}

public class FormPresenter : IFormPresenter
{
    public virtual string IndexViewName { get; set; } = "ShowPage";
    public virtual string StopViewName { get; set; } = "Stop";

    public virtual string SummaryViewName { get; set; } = "Summary";

    public virtual string ConfirmationViewName { get; set; } = "Confirmation";   

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
                NextAction = SummaryViewName
            };
        }

        if (!String.IsNullOrEmpty(currentPage.PageType) && currentPage.PageType.Equals("stop"))
        {
            return new SubmitResult()
            {
                NextAction = StopViewName,
                NextPage = response.NextPageId
            };
        }

        return new SubmitResult
        {
            NextAction = IndexViewName,
            NextPage = nextPageId,
            ExtraData = extraData
        };
    }

    public virtual async Task<SummaryResult> HandleSummary(FormModel formModel, GetDataResponseModel response)
    {
        var formData = new Dictionary<string, object>();
        
        if (!String.IsNullOrEmpty(response.FormData.Data))
        {
            formData = JsonConvert.DeserializeObject<Dictionary<string, object>>(response.FormData.Data);
        }

        foreach (var page in formModel.Pages) 
        {
            foreach (var component in page.Components.Where(c => c.IsQuestionType))
            {
                if (formData.TryGetValue(component.Name, out object? value))
                {
                    var handler = _componentHandlerFactory.GetFor(component.Type);
                    component.Answer = handler.GetFromObject(value);
                }
            }
        }

        return new SummaryResult
        {
            FormModel = formModel,
            ViewName = SummaryViewName
        };
    }

    public virtual async Task<StopResult> HandleStop(string pageId, FormModel formModel)
    {
        var currentPage = formModel.Pages.Find(p => p.PageId == pageId);
        if (currentPage == null) return null;

        return new StopResult
        {
            ViewName = StopViewName,
            PageModel = currentPage
        };
    }

    public virtual async Task<ConfirmationResult> HandleConfirmation(string applicationId)
    {
        return new ConfirmationResult
        {
            ApplicationId = applicationId,
            ViewName = ConfirmationViewName
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
    public FormModel FormModel { get; set; }
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