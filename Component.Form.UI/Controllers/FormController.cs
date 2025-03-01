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

    [HttpGet("form/{formId}/{page}")]
    public async Task<IActionResult> Index(string formId, int page)
    {
        var formModel = await _formAPIService.GetFormAsync(formId);
        if (formModel == null) return NotFound();

        var result = await _formPresenter.HandleIndex(page, formModel);
        if (result == null) return NotFound();

        ViewBag.CurrentPage = result.CurrentPage;
        ViewBag.TotalPages = result.TotalPages;
        ViewBag.FormId = formId;
        return View(result.NextAction, result.PageModel);
    }

    [HttpPost]
    public async Task<IActionResult> Submit()
    {
        var formModel = await _formAPIService.GetFormAsync(Request.Form["FormId"]);
        var result = await _formPresenter.HandleSubmit(Request, formModel);

        if (result.Errors.Any())
        {
            ViewBag.Errors = result.Errors;
            ViewBag.FormId = formModel.FormId;
            ViewBag.CurrentPage = result.CurrentPage;
            ViewBag.TotalPages = formModel.TotalPages;
            return View(result.NextAction, result.PageModel);
        }

        return RedirectToAction(result.NextAction, new { formId = formModel.FormId, page = result.NextPage });
    }

    public async Task<IActionResult> ThankYou()
    {
        return View();
    }
}