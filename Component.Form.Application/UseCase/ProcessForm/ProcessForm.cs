using Component.Core.Application;
using Component.Form.Application.Shared.Infrastructure;
using Component.Form.Application.UseCase.ProcessForm.Model;
using Newtonsoft.Json;
using Component.Form.Application.Helpers;
using Component.Form.Application.ComponentHandler;
using Component.Form.Application.PageHandler;
namespace Component.Form.Application.UseCase.ProcessForm;

public class ProcessForm : IRequestResponseUseCase<ProcessFormRequestModel, ProcessFormResponseModel>
{
    private IFormStore _formStore;
    private readonly IFormDataStore _formDataStore;
    private readonly IComponentHandlerFactory _componentHandlerFactory;
    private readonly IPageHandlerFactory _pageHandlerFactory;
    private readonly SafeJsonHelper _safeJsonHelper; 
    
    public ProcessForm(
        IPageHandlerFactory pageHandlerFactory, 
        IComponentHandlerFactory componentHandlerFactory, 
        IFormStore formStore, 
        IFormDataStore formDataStore,
        SafeJsonHelper safeJsonHelper)
    {
        _formStore = formStore;
        _formDataStore = formDataStore;
        _componentHandlerFactory = componentHandlerFactory;
        _pageHandlerFactory = pageHandlerFactory;
        _safeJsonHelper = safeJsonHelper;
    }

    public async Task<ProcessFormResponseModel> HandleAsync(ProcessFormRequestModel request)
    {
        var form = await _formStore.GetFormAsync(request.FormId);

        if (form == null)
        {
            throw new ArgumentException($"Form {request.FormId} not found");
        }

        var basePage = form.Pages.FirstOrDefault(p => p.PageId == request.PageId);

        if (basePage == null) 
        {
            throw new ArgumentException($"Page {request.PageId} not found");
        }

        var pageHandler = _pageHandlerFactory.GetFor(basePage.PageType);

        if (pageHandler == null)
        {
            throw new ArgumentException($"Page handler not found for page type {basePage.PageType}");
        }

        var formDataModel = await _formDataStore.GetFormDataAsync(request.ApplicantId);

        var formData = formDataModel.Data;
        
        dynamic data = FormHelper.ParseData(formData, _componentHandlerFactory);

        var response = await pageHandler.Process(basePage, data, request.Form);

        // store the current form data. errors and all...
        await _formDataStore.SaveFormDataAsync(request.FormId, request.ApplicantId, _safeJsonHelper.SafeSerializeObject(response.Data));

        return new ProcessFormResponseModel()
        {
            NextPageId = response.NextPageId,
            ExtraData = response.ExtraData
        };  
    }       
}