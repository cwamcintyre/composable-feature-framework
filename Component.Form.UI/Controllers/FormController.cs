using Component.Form.UI.Helpers;
using Component.Form.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class FormController : Controller
{
    private readonly FormAPIService _formAPIService;
    private readonly FormHelper _formHelper;
    private readonly IFormPresenter _formPresenter;

    public FormController(IFormPresenter formPresenter, FormAPIService formAPIService, FormHelper formHelper)
    {
        _formAPIService = formAPIService;
        _formPresenter = formPresenter;        
        _formHelper = formHelper;
    }

    [HttpGet("form/{formId}/start")]
    public async Task<IActionResult> Start(string formId)
    {
        var formModel = await _formAPIService.GetFormAsync(formId);
        if (formModel == null) return NotFound();

        // ensure a clean session and generate a new applicant ID.
        FormSessionHelper.ClearApplicantId(HttpContext.Session);
        FormSessionHelper.SetApplicantId(HttpContext.Session, FormSessionHelper.GenerateApplicantId());

        return RedirectToAction("ShowPage", new { formId = formModel.FormId, page = formModel.StartPage });
    }

    [HttpGet("form/{formId}/{page}")]
    public async Task<IActionResult> ShowPage(string formId, string page)
    {
        var formModel = await _formAPIService.GetFormAsync(formId);
        if (formModel == null) return NotFound();

        var data = await _formAPIService.GetFormDataAsync(FormSessionHelper.GetApplicantId(HttpContext.Session));

        var result = await _formPresenter.HandlePage(page, formModel, data);
        if (result == null) return NotFound();

        ViewBag.CurrentPage = result.CurrentPage;
        ViewBag.TotalPages = result.TotalPages;
        ViewBag.FormId = formId;
        ViewBag.PreviousPageId = result.PreviousPage;
        return View(result.NextAction, result.PageModel);
    }

    [HttpPost]
    public async Task<IActionResult> Submit()
    {
        var formModel = await _formAPIService.GetFormAsync(Request.Form["FormId"]);
        var currentPage = formModel.Pages.Find(p => p.PageId == Request.Form["PageId"]);
        if (currentPage == null) 
        {
            throw new ArgumentException($"Page {Request.Form["PageId"]} not found in form {Request.Form["FormId"]}");    
        }

        var formData = _formHelper.GetSubmittedPageData(currentPage, Request.Form.ToDictionary(x => x.Key, x => x.Value.ToString()));
        
        var processResult = await _formAPIService.ProcessFormAsync(
            Request.Form["FormId"],
            FormSessionHelper.GetApplicantId(HttpContext.Session),
            Request.Form["PageId"],
            formData);

        var result = await _formPresenter.HandleSubmit(formModel, processResult);

        if (result.Errors != null && result.Errors.Any())
        {
            ViewBag.Errors = result.Errors;
            ViewBag.FormId = formModel.FormId;
            return View(result.NextAction, result.PageModel);
        }

        return RedirectToAction(result.NextAction, new { formId = formModel.FormId, page = result.NextPage });
    }

    [HttpGet("form/{formId}/summary")]
    public async Task<IActionResult> Summary(string formId)
    {
        var formModel = await _formAPIService.GetFormAsync(formId);
        if (formModel == null) return NotFound();

        var data = await _formAPIService.GetFormDataAsync(FormSessionHelper.GetApplicantId(HttpContext.Session));

        var result = await _formPresenter.HandleSummary(formModel, data);

        ViewBag.FormId = formId;
        return View(result.NextAction, formModel);
    }

    [HttpGet("form/{formId}/{page}/stop")]
    public async Task<IActionResult> Stop(string formId, string page)
    {
        var formModel = await _formAPIService.GetFormAsync(formId);
        if (formModel == null) return NotFound();

        var result = await _formPresenter.HandleStop(page, formModel);
        return View(result.NextAction, result.PageModel);
    }

    [HttpGet("form/{formId}/confirmation")]
    public async Task<IActionResult> Confirmation()
    {
        return View();
    }
}