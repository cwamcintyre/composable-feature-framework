using Component.Form.UI.Helpers;
using Component.Form.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Component.Form.UI.PageHandler;
using Component.Form.UI.Exceptions;

namespace Component.Form.UI.Controllers
{
    public class FormController : Controller
    {
        private readonly ILogger<FormController> _logger;
        private readonly FormAPIService _formAPIService;
        private readonly IFormPresenter _formPresenter;
        private readonly PageHandlerFactory _pageHanderFactory;
        
        public FormController(
            IFormPresenter formPresenter, 
            FormAPIService formAPIService, 
            PageHandlerFactory pageHanderFactory, 
            ILogger<FormController> logger)
        {
            _formAPIService = formAPIService;
            _formPresenter = formPresenter;
            _pageHanderFactory = pageHanderFactory;
            _logger = logger;
        }

        [HttpGet("form/{formId}/start")]
        public async Task<IActionResult> Start(string formId)
        {
            if (string.IsNullOrEmpty(formId))
            {
                throw new BadRouteParametersException("FormId is required.");
            }
            var formModel = await _formAPIService.GetFormAsync(formId);
            if (formModel == null) throw new ArgumentNullException($"Form {formId} not found.");

            // ensure a clean session and generate a new applicant ID.
            FormSessionHelper.ClearApplicantId(HttpContext.Session);
            FormSessionHelper.SetApplicantId(HttpContext.Session, FormSessionHelper.GenerateApplicantId());

            return RedirectToAction("ShowPage", new { formId = formModel.FormId, page = formModel.StartPage });
        }

        [HttpGet("form/{formId}/{page}/{*extraData}")]
        public async Task<IActionResult> ShowPage(string formId, string page, string extraData)
        {   
            if (string.IsNullOrEmpty(formId) || string.IsNullOrEmpty(page))
            {
                throw new BadRouteParametersException("FormId and Page are required.");
            }
            var formModel = await _formAPIService.GetFormAsync(formId);
            if (formModel == null) throw new ArgumentNullException($"Form {formId} not found.");

            var currentPage = formModel.Pages.Find(p => p.PageId == page);
            if (currentPage == null) throw new ArgumentNullException($"Page {page} not found.");
            
            var data = await _formAPIService.GetFormDataForPageAsync(formId, page, FormSessionHelper.GetApplicantId(HttpContext.Session), extraData);           

            if (data.ForceRedirect)
            {
                return RedirectToAction("ShowPage", new { formId = formId, page = data.PreviousPage, extraData = data.PreviousExtraData });
            }

            var pageHandler = _pageHanderFactory.GetFor(currentPage.PageType);
            if (pageHandler == null) throw new ArgumentNullException($"Page handler {currentPage.PageType} not found.");

            var result = await pageHandler.HandlePage(currentPage, data, extraData);

            ViewBag.FormId = formId;
            ViewBag.PreviousPageId = result.PreviousPage;
            ViewBag.PreviousExtraData = result.PreviousExtraData;
            ViewBag.Errors = result.Errors;

            return View(result.ViewName, result.PageModel);
        }

        [HttpPost]
        [RequestSizeLimit(512 * 1024 * 1024)]
        public async Task<IActionResult> Submit()
        {
            var formId = Request.Form["FormId"];
            var pageId = Request.Form["PageId"];
        
            if (String.IsNullOrEmpty(formId) || String.IsNullOrEmpty(pageId))
            {
                throw new BadRouteParametersException("FormId or PageId is null");
            }

            var formModel = await _formAPIService.GetFormAsync(formId);
            var currentPage = formModel.Pages.Find(p => p.PageId == pageId);
            if (currentPage == null) throw new ArgumentNullException($"Form {formId} not found.");

            var pageHandler = _pageHanderFactory.GetFor(currentPage.PageType);
            if (pageHandler == null) throw new ArgumentNullException($"Page handler {currentPage.PageType} not found.");

            var formData = await pageHandler.GetSubmittedPageData(currentPage, Request.Form.ToDictionary(x => x.Key, x => x.Value.ToString()));
            
            // Check if there are any submitted files
            if (Request.Form.Files.Count > 0)
            {
                var fileComponent = currentPage.Components.FirstOrDefault(c => c.Type == "fileupload");
                if (fileComponent == null)
                {
                    ModelState.AddModelError(string.Empty, "File upload component not found but files in the request.");
                    ViewBag.FormId = formId;
                    return View("Error");
                }

                foreach (var file in Request.Form.Files)
                {
                    var response = await _formAPIService.StreamFileToExternalApi(file, fileComponent.FileOptions.UploadEndpoint);
                }
            }

            var processResult = await _formAPIService.ProcessFormAsync(
                formId,
                FormSessionHelper.GetApplicantId(HttpContext.Session),
                pageId,
                formData);
            
            var result = await _formPresenter.HandleSubmit(formModel, processResult);

            return RedirectToAction(result.NextAction, new { formId = formModel.FormId, page = result.NextPage, extraData = result.ExtraData });
        }

        [HttpGet("form/{formId}/{page}/change/{*extraData}")]
        public async Task<IActionResult> ShowChangePage(string formId, string page, string extraData)
        {   
            if (string.IsNullOrEmpty(formId) || string.IsNullOrEmpty(page))
            {
                throw new BadRouteParametersException("FormId and Page are required.");
            }
            var formModel = await _formAPIService.GetFormAsync(formId);
            if (formModel == null) throw new ArgumentNullException($"Form {formId} not found.");

            var currentPage = formModel.Pages.Find(p => p.PageId == page);
            if (currentPage == null) throw new ArgumentNullException($"Page {page} not found.");
            
            var data = await _formAPIService.GetFormDataForPageAsync(formId, page, FormSessionHelper.GetApplicantId(HttpContext.Session), extraData);           

            if (data.ForceRedirect)
            {
                return RedirectToAction("ShowPage", new { formId = formId, page = data.PreviousPage, extraData = data.PreviousExtraData });
            }

            var pageHandler = _pageHanderFactory.GetFor(currentPage.PageType);
            if (pageHandler == null) throw new ArgumentNullException($"Page handler {currentPage.PageType} not found.");

            var result = await pageHandler.HandlePage(currentPage, data, extraData);

            ViewBag.FormId = formId;
            ViewBag.PreviousPageId = result.PreviousPage;
            ViewBag.PreviousExtraData = result.PreviousExtraData;
            ViewBag.Errors = result.Errors;
            ViewBag.IsChange = true;
            ViewBag.ExtraData = extraData;

            return View(result.ViewName, result.PageModel);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitChange()
        {
            var formId = Request.Form["FormId"];
            var pageId = Request.Form["PageId"];
        
            if (String.IsNullOrEmpty(formId) || String.IsNullOrEmpty(pageId))
            {
                throw new BadRouteParametersException("FormId or PageId is null");
            }

            var formModel = await _formAPIService.GetFormAsync(formId);
            var currentPage = formModel.Pages.Find(p => p.PageId == pageId);
            if (currentPage == null) throw new ArgumentNullException($"Form {formId} not found.");

            var pageHandler = _pageHanderFactory.GetFor(currentPage.PageType);
            if (pageHandler == null) throw new ArgumentNullException($"Page handler {currentPage.PageType} not found.");

            var formData = await pageHandler.GetSubmittedPageData(currentPage, Request.Form.ToDictionary(x => x.Key, x => x.Value.ToString()));
            
            // Check if there are any submitted files
            if (Request.Form.Files.Count > 0)
            {
                var fileComponent = currentPage.Components.FirstOrDefault(c => c.Type == "fileupload");
                if (fileComponent == null)
                {
                    ModelState.AddModelError(string.Empty, "File upload component not found but files in the request.");
                    ViewBag.FormId = formId;
                    return View("Error");
                }

                foreach (var file in Request.Form.Files)
                {
                    var response = await _formAPIService.StreamFileToExternalApi(file, fileComponent.FileOptions.UploadEndpoint);
                }
            }

            var processResult = await _formAPIService.ProcessChangeAsync(
                formId,
                FormSessionHelper.GetApplicantId(HttpContext.Session),
                pageId,
                formData);
            
            var result = await _formPresenter.HandleChangeSubmit(formModel, processResult);

            return RedirectToAction(result.NextAction, new { formId = formModel.FormId, page = result.NextPage, extraData = result.ExtraData });
        }        

        [HttpGet("form/{formId}/summary")]
        public async Task<IActionResult> Summary(string formId)
        {
            if (string.IsNullOrEmpty(formId))
            {
                return BadRequest("FormId is required.");
            }
            var formModel = await _formAPIService.GetFormAsync(formId);
            if (formModel == null) throw new ArgumentNullException($"Form {formId} not found.");

            var data = await _formAPIService.GetFormDataAsync(FormSessionHelper.GetApplicantId(HttpContext.Session));

            var result = await _formPresenter.HandleSummary(formModel, data);

            ViewBag.FormId = formId;
            return View(result.ViewName, result.SummaryModel);
        }

        [HttpGet("form/{formId}/{page}/stop")]
        public async Task<IActionResult> Stop(string formId, string page)
        {
            if (string.IsNullOrEmpty(formId) || string.IsNullOrEmpty(page))
            {
                throw new BadRouteParametersException("FormId and Page are required.");
            }
            var formModel = await _formAPIService.GetFormAsync(formId);
            if (formModel == null) throw new ArgumentNullException($"Form {formId} not found.");

            var result = await _formPresenter.HandleStop(page, formModel);
            return View(result.ViewName, result.PageModel);
        }

        [HttpGet("form/{formId}/confirmation")]
        public async Task<IActionResult> Confirmation(string formId)
        {
            var formModel = await _formAPIService.GetFormAsync(formId);
            if (formModel == null) throw new ArgumentNullException($"Form {formId} not found.");

            var applicationId = FormSessionHelper.GetApplicantId(HttpContext.Session);
            var data = await _formAPIService.GetFormDataAsync(applicationId);

            // TODO: mark the application as submitted if this resolves ok?
            await _formAPIService.SubmitFormDataAsync(formModel, data.FormData);

            var result = await _formPresenter.HandleConfirmation(applicationId);

            return View(result.ViewName, result.ApplicationId);
        }
    }
}