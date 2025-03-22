using Component.Form.Model;
using Component.Form.UI.Models;
using Component.Form.UI.Services.Model;

namespace Component.Form.UI.PageHandler;

public interface IPageHandler
{
    bool IsFor(string type);
    Task<ShowResult> HandlePage(PageBase page, GetDataForPageResponseModel dataModel, string extraData);
    Task<Dictionary<string, string>> GetSubmittedPageData(PageBase page, Dictionary<string, string> formData);
    Task<PageSummaryItemViewModelBase> GetSummaryItem(PageBase page, Dictionary<string, object> formData);
}

public class ShowResult
{
    public PageBase PageModel { get; set; }
    public string PreviousPage { get; set; }
    public string PreviousExtraData { get; set; }
    public string ViewName { get; set; }
    public Dictionary<string, List<string>> Errors { get; set; } = new Dictionary<string, List<string>>();
}

public class PageResult
{
    public string NextPage { get; set; }
    public string ViewName { get; set; }
    public string ExtraData { get; set; }
}