using Component.Form.UI.Helpers;
using Component.Form.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Component.Form.Model;
using System.Net.Http.Headers;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using Component.Form.UI.PageHandler;

namespace Component.Form.UI.Controllers
{
    public class FormController : Controller
    {
        public const string TempData_ReturnWithErrorsKey = "ReturnWithErrors";
        public const string TempData_ErrorsKey = "Errors";

        private readonly FormAPIService _formAPIService;
        private readonly IFormPresenter _formPresenter;
        private readonly PageHandlerFactory _pageHanderFactory;
        
        public FormController(
            IFormPresenter formPresenter, 
            FormAPIService formAPIService, 
            PageHandlerFactory pageHanderFactory)
        {
            _formAPIService = formAPIService;
            _formPresenter = formPresenter;
            _pageHanderFactory = pageHanderFactory;
        }

        [HttpGet("form/{formId}/start")]
        public async Task<IActionResult> Start(string formId)
        {
            if (string.IsNullOrEmpty(formId))
            {
                return BadRequest("FormId is required.");
            }
            var formModel = await _formAPIService.GetFormAsync(formId);
            if (formModel == null) return NotFound();

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
                return BadRequest("FormId and Page are required.");
            }
            var formModel = await _formAPIService.GetFormAsync(formId);
            if (formModel == null) return NotFound();

            var currentPage = formModel.Pages.Find(p => p.PageId == page);
            if (currentPage == null) return NotFound();
            
            var data = await _formAPIService.GetFormDataForPageAsync(formId, page, FormSessionHelper.GetApplicantId(HttpContext.Session), extraData);           

            var pageHandler = _pageHanderFactory.GetFor(currentPage.PageType);
            if (pageHandler == null) return NotFound();

            var result = await pageHandler.HandlePage(currentPage, data, extraData);

            ViewBag.FormId = formId;
            ViewBag.PreviousPageId = result.PreviousPage;
            ViewBag.PreviousExtraData = result.PreviousExtraData;
            ViewBag.Errors = result.Errors;

            return View(result.ViewName, result.PageModel);
        }

        [HttpPost]
        public async Task<IActionResult> Submit()
        {
            var formId = Request.Form["FormId"];
            var pageId = Request.Form["PageId"];
        
            if (String.IsNullOrEmpty(formId) || String.IsNullOrEmpty(pageId))
            {
                throw new ArgumentException("FormId or PageId is null");
            }

            var formModel = await _formAPIService.GetFormAsync(formId);
            var currentPage = formModel.Pages.Find(p => p.PageId == pageId);
            if (currentPage == null) return NotFound();

            var pageHandler = _pageHanderFactory.GetFor(currentPage.PageType);
            if (pageHandler == null) return NotFound();

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
                    // Find the file upload component and post the file to the UploadEndpoint
                    using (var stream = new MemoryStream())
                    {
                        await file.CopyToAsync(stream);
                        stream.Position = 0;
                        var fileContent = new ByteArrayContent(stream.ToArray());
                        fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);

                        using (var client = new HttpClient())
                        {
                            var fileFormData = new MultipartFormDataContent
                            {
                                { fileContent, "files", file.FileName }
                            };

                            var response = await client.PostAsync(fileComponent.FileOptions.UploadEndpoint, fileFormData);
                            if (!response.IsSuccessStatusCode)
                            {
                                ModelState.AddModelError(string.Empty, "File upload failed.");
                                ViewBag.FormId = formId;
                                return View("Error");
                            }

                            // TODO: add a property to the component to store multiple file names.
                            // BUG: the file name is not stored in the data...
                            fileComponent.Answer = file.FileName;
                        }
                    }
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

        [HttpGet("form/{formId}/summary")]
        public async Task<IActionResult> Summary(string formId)
        {
            if (string.IsNullOrEmpty(formId))
            {
                return BadRequest("FormId is required.");
            }
            var formModel = await _formAPIService.GetFormAsync(formId);
            if (formModel == null) return NotFound();

            var data = await _formAPIService.GetFormDataAsync(FormSessionHelper.GetApplicantId(HttpContext.Session));

            var result = await _formPresenter.HandleSummary(formModel, data);

            ViewBag.FormId = formId;
            return View(result.ViewName, formModel);
        }

        [HttpGet("form/{formId}/{page}/stop")]
        public async Task<IActionResult> Stop(string formId, string page)
        {
            if (string.IsNullOrEmpty(formId) || string.IsNullOrEmpty(page))
            {
                return BadRequest("FormId and Page are required.");
            }
            var formModel = await _formAPIService.GetFormAsync(formId);
            if (formModel == null) return NotFound();

            var result = await _formPresenter.HandleStop(page, formModel);
            return View(result.ViewName, result.PageModel);
        }

        [HttpGet("form/{formId}/confirmation")]
        public async Task<IActionResult> Confirmation(string formId)
        {
            var formModel = await _formAPIService.GetFormAsync(formId);
            if (formModel == null) return NotFound();

            var applicationId = FormSessionHelper.GetApplicantId(HttpContext.Session);
            var data = await _formAPIService.GetFormDataAsync(applicationId);

            // TODO: mark the application as submitted if this resolves ok?
            await _formAPIService.SubmitFormDataAsync(formModel, data.FormData);

            var result = await _formPresenter.HandleConfirmation(applicationId);

            return View(result.ViewName, result.ApplicationId);
        }
    }
}