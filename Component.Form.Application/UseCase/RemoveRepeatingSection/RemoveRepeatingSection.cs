using System.Dynamic;
using Component.Core.Application;
using Component.Form.Application.Helpers;
using Component.Form.Application.PageHandler;
using Component.Form.Application.Shared.Infrastructure;
using Component.Form.Application.UseCase.RemoveRepeatingSection.Model;
using Component.Form.Model.PageHandler;

namespace Component.Form.Application.UseCase.RemoveRepeatingSection;

public class RemoveRepeatingSection : IRequestResponseUseCase<RemoveRepeatingSectionRequestModel, RemoveRepeatingSectionResponseModel>
{
    private readonly IFormStore _formStore;
    private readonly IFormDataStore _formDataStore;
    private readonly IPageHandlerFactory _pageHandlerFactory;
    private readonly SafeJsonHelper _safeJsonHelper;

    public RemoveRepeatingSection(IFormStore formStore, IFormDataStore formDataStore, IPageHandlerFactory pageHandlerFactory, SafeJsonHelper safeJsonHelper)
    {
        _formStore = formStore;
        _formDataStore = formDataStore;
        _safeJsonHelper = safeJsonHelper;
        _pageHandlerFactory = pageHandlerFactory;
    }

    public async Task<RemoveRepeatingSectionResponseModel> HandleAsync(RemoveRepeatingSectionRequestModel request)
    {
        if (request.FormId == null)
        {
            throw new ArgumentNullException(nameof(request.FormId));
        }

        var form = await _formStore.GetFormAsync(request.FormId);
        var formDataModel = await _formDataStore.GetFormDataAsync(request.ApplicantId);

        if (form == null)
        {
            throw new ArgumentException($"Form {request.FormId} not found");
        }

        var page = form.Pages.Find(p => p.PageId == request.PageId);

        if (page == null)
        {
            throw new ArgumentException($"Page {request.PageId} not found");
        }

        var formData = _safeJsonHelper.SafeDeserializeObject<ExpandoObject>(formDataModel.Data);
        var formDataDict = formData as IDictionary<string, object>;

        var repeatingPage = page as InlineRepeatingPageSection;
        if (repeatingPage == null)
        {
            throw new ArgumentException($"Page {request.PageId} is not a repeating page");
        }

        if (formDataDict == null || !formDataDict.ContainsKey(repeatingPage.RepeatKey))
        {
            throw new ArgumentException($"No data found for repeating page {repeatingPage.RepeatKey}");
        }

        var repeatList = formDataDict[repeatingPage.RepeatKey] as List<object>;

        if (repeatList == null || repeatList.Count <= request.Index)
        {
            throw new ArgumentException($"No data found for repeating page at index {request.Index}");
        }

        repeatList.RemoveAt(request.Index);

        var pageHandler = _pageHandlerFactory.GetFor(page.PageType);
        if (pageHandler == null)
        {
            throw new ArgumentException($"Page handler not found for page type {page.PageType}");
        }

        var meetsCondition = repeatingPage.DataThatMeetsCondition;
        var doesNotMeetCondition = repeatingPage.DataThatDoesNotMeetCondition;

        for (int i = 0; i < repeatList.Count; i++)
        {
            var repeatListData = repeatList[i] as IDictionary<string, object>;
            if (i < repeatList.Count - 1)
            {
                foreach (var kvPair in meetsCondition)
                {
                    if (repeatListData.ContainsKey(kvPair.Key))
                    {
                        repeatListData[kvPair.Key] = kvPair.Value;
                    }
                }
            }
            else
            {
                foreach (var kvPair in doesNotMeetCondition)
                {
                    if (repeatListData.ContainsKey(kvPair.Key))
                    {
                        repeatListData[kvPair.Key] = kvPair.Value;
                    }
                }
            }
        }

        await _formDataStore.SaveFormDataAsync(request.FormId, request.ApplicantId, _safeJsonHelper.SafeSerializeObject(formDataDict));

        return new RemoveRepeatingSectionResponseModel { Success = true };
    }
}