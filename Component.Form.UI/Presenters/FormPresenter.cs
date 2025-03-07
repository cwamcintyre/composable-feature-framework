using Component.Form.Model;
using Component.Form.Model.ComponentHandler;
using Component.Form.UI.Services.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public interface IFormPresenter
{
    Task<IndexResult> HandlePage(string page, FormModel formModel, GetDataResponseModel dataModel);
    Task<StopResult> HandleStop(string pageId, FormModel formModel);
    Task<SubmitResult> HandleSubmit(FormModel formModel, ProcessFormResponseModel response);
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

    public FormPresenter(ComponentHandlerFactory componentHandlerFactory)
    {
        _componentHandlerFactory = componentHandlerFactory;
    }

    public virtual async Task<IndexResult> HandlePage(string page, FormModel formModel, GetDataResponseModel dataResponse)
    {
        var currentPage = formModel.Pages.Find(p => p.PageId == page);
        if (currentPage == null) return null;

        var formData = new Dictionary<string, object>();
        
        if (!String.IsNullOrEmpty(dataResponse.FormData.Data))
        {
            formData = JsonConvert.DeserializeObject<Dictionary<string, object>>(dataResponse.FormData.Data, GetJsonSerializerSettings());
        }
        
        if (dataResponse.FormData.Data != null) 
        {
            foreach (var data in formData)
            {
                var component = currentPage.Components.Find(c => c.Name == data.Key);
                if (component != null)
                {
                    var handler = _componentHandlerFactory.GetFor(component.Type);
                    component.Answer = handler.GetFromObject(data.Value);
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

    public virtual async Task<SubmitResult> HandleSubmit(FormModel formModel, ProcessFormResponseModel response)
    {
        var currentPage = formModel.Pages.Find(p => p.PageId == response.NextPage);
        if (currentPage == null) return null;

        var formData = new Dictionary<string, object>();
        
        if (!String.IsNullOrEmpty(response.FormData))
        {
            formData = JsonConvert.DeserializeObject<Dictionary<string, object>>(response.FormData, GetJsonSerializerSettings());
        }

        if (!String.IsNullOrEmpty(currentPage.PageType) && currentPage.PageType.Equals("summary"))
        {
            return new SubmitResult()
            {
                NextAction = SummaryViewName,
                NextPage = response.NextPage
            };
        }

        if (!String.IsNullOrEmpty(currentPage.PageType) && currentPage.PageType.Equals("stop"))
        {
            return new SubmitResult()
            {
                NextAction = StopViewName,
                NextPage = response.NextPage
            };
        }

        foreach (var data in formData)
        {
            var component = currentPage.Components.Find(c => c.Name == data.Key);
            if (component != null)
            {
                var handler = _componentHandlerFactory.GetFor(component.Type);
                component.Answer = handler.GetFromObject(data.Value);
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

    public virtual async Task<SummaryResult> HandleSummary(FormModel formModel, GetDataResponseModel response)
    {
        var formData = new Dictionary<string, object>();
        
        if (!String.IsNullOrEmpty(response.FormData.Data))
        {
            formData = JsonConvert.DeserializeObject<Dictionary<string, object>>(response.FormData.Data, GetJsonSerializerSettings());
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
            NextAction = SummaryViewName
        };
    }

    public virtual async Task<StopResult> HandleStop(string pageId, FormModel formModel)
    {
        var currentPage = formModel.Pages.Find(p => p.PageId == pageId);
        if (currentPage == null) return null;

        return new StopResult
        {
            NextAction = StopViewName,
            PageModel = currentPage
        };
    }

    public virtual async Task<ConfirmationResult> HandleConfirmation(string applicationId)
    {
        return new ConfirmationResult
        {
            ApplicationId = applicationId,
            NextAction = ConfirmationViewName
        };
    }

    private JsonSerializerSettings GetJsonSerializerSettings()
    {
        return new JsonSerializerSettings
        {
            SerializationBinder = new SafeTypeResolver(_componentHandlerFactory.GetAllTypes())
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
    public Dictionary<string, List<string>> Errors { get; set; } = new Dictionary<string, List<string>>();
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

public class ConfirmationResult
{
    public string ApplicationId { get; set; }
    public string NextAction { get; set; }
}