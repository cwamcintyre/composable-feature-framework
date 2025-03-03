using Component.Form.UI.Helpers;
using Component.Form.UI.Services;
using Microsoft.AspNetCore.Mvc;

public class FormController : Controller
{
    private readonly FormAPIService _formAPIService;
    private readonly IFormPresenter _formPresenter;

    public FormController(IFormPresenter formPresenter, FormAPIService formAPIService)
    {
        _formAPIService = formAPIService;
        _formPresenter = formPresenter;        
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
        var processResult = await _formAPIService.ProcessFormAsync(
            Request.Form["FormId"],
            FormSessionHelper.GetApplicantId(HttpContext.Session),
            Request.Form["PageId"],
            Request.Form.ToDictionary(f => f.Key, f => f.Value.ToString()));

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