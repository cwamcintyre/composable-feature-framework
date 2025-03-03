using Component.Form.Model;
using Component.Form.UI.Services.Model;

public interface IFormPresenter
{
    Task<IndexResult> HandlePage(string page, FormModel formModel, GetDataResponseModel dataModel);
    Task<StopResult> HandleStop(string pageId, FormModel formModel);
    Task<SubmitResult> HandleSubmit(FormModel formModel, ProcessFormResponseModel response);
    Task<SummaryResult> HandleSummary(FormModel formModel, GetDataResponseModel response);
}

public class FormPresenter : IFormPresenter
{
    public virtual string IndexViewName { get; set; } = "ShowPage";
    public virtual string StopViewName { get; set; } = "Stop";

    public async Task<IndexResult> HandlePage(string page, FormModel formModel, GetDataResponseModel dataResponse)
    {
        var currentPage = formModel.Pages.Find(p => p.PageId == page);
        if (currentPage == null) return null;

        if (dataResponse.FormData.Data != null) 
        {
            foreach (var formData in dataResponse.FormData.Data)
            {
                var component = currentPage.Components.Find(c => c.Name == formData.Key);
                if (component != null)
                {
                    component.Answer = formData.Value;
                }
            }
        }

        var previousPage = "";
        if (dataResponse.FormData.Route != null && dataResponse.FormData.Route.Count > 0) 
        {
            previousPage = dataResponse.FormData.Route.Peek();
        }

        return new IndexResult
        {
            PageModel = currentPage,
            CurrentPage = page,
            TotalPages = formModel.TotalPages,
            NextAction = IndexViewName,
            PreviousPage = previousPage
        };
    }

    public async Task<SubmitResult> HandleSubmit(FormModel formModel, ProcessFormResponseModel response)
    {
        var currentPage = formModel.Pages.Find(p => p.PageId == response.NextPage);
        if (currentPage == null) return null;

        if (!String.IsNullOrEmpty(currentPage.PageType) && currentPage.PageType.Equals("summary"))
        {
            return new SubmitResult()
            {
                NextAction = "Summary",
                NextPage = response.NextPage
            };
        }

        if (!String.IsNullOrEmpty(currentPage.PageType) && currentPage.PageType.Equals("stop"))
        {
            return new SubmitResult()
            {
                NextAction = "Stop",
                NextPage = response.NextPage
            };
        }

        foreach (var formData in response.FormData)
        {
            var component = currentPage.Components.Find(c => c.Name == formData.Key);
            if (component != null)
            {
                component.Answer = formData.Value;
            }
        }

        var errors = response.Errors;
        var nextPage = response.NextPage;

        return new SubmitResult
        {
            Errors = errors,
            NextPage = nextPage,
            PageModel = currentPage,
            NextAction = IndexViewName
        };
    }

    public async Task<SummaryResult> HandleSummary(FormModel formModel, GetDataResponseModel response)
    {        
        var data = response.FormData.Data;

        foreach (var page in formModel.Pages) 
        {
            foreach (var component in page.Components.Where(c => c.IsQuestionType))
            {
                if (data.ContainsKey(component.Name))
                {
                    component.Answer = data[component.Name];
                }
            }
        }

        return new SummaryResult
        {
            FormModel = formModel,
            NextAction = "Summary"
        };
    }

    public async Task<StopResult> HandleStop(string pageId, FormModel formModel)
    {
        var currentPage = formModel.Pages.Find(p => p.PageId == pageId);
        if (currentPage == null) return null;

        return new StopResult
        {
            NextAction = StopViewName,
            PageModel = currentPage
        };
    }
}

public class IndexResult
{
    public Page PageModel { get; set; }
    public string CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public string NextAction { get; set; }
    public string PreviousPage { get; set; }
}

public class SubmitResult
{
    public Dictionary<string, string> Errors { get; set; } = new Dictionary<string, string>();
    public string NextPage { get; set; }
    public Page PageModel { get; set;}
    public string NextAction { get; set; }
}

public class SummaryResult
{
    public FormModel FormModel { get; set; }
    public string NextAction { get; set; }
}

public class StopResult
{
    public Page PageModel { get; set; }
    public string NextAction { get; set; }
}