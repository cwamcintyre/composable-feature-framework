using System;
using Component.Form.Application.UseCase.GetDataForPage.Model;
using Component.Form.Application.UseCase.ProcessForm.Model;
using Component.Form.Model;

namespace Component.Form.Application.PageHandler;

public interface IPageHandler
{
    bool IsFor(string type);
    Task<ProcessFormResponseModel> Process(PageBase page, dynamic existingData, Dictionary<string, string> formData);
    Task<NextPageIdResult> GetNextPageId(PageBase page, dynamic data, string extraData);
    Task<GetDataForPageResponseModel> Get(PageBase page, dynamic data, string extraData);
}

public class NextPageIdResult
{
    public string NextPageId { get; set; }
    public string ExtraData { get; set; }
}
