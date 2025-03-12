using Component.Form.UI.Helpers;
using Component.Form.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Component.Form.Model;
using System.Net.Http.Headers;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;

namespace Component.Form.UI.Controllers
{
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

        [HttpGet("form/{formId}/list")]
        public async Task<IActionResult> Index(string formId)
        {
            if (string.IsNullOrEmpty(formId))
            {
                return BadRequest("FormId is required.");
            }
            var form = await _formAPIService.GetFormAsync(formId);
            ViewBag.FormId = formId;
            return View("ListForm", form);
        }

        [HttpGet("form/{formId}/edit")]
        public async Task<IActionResult> Edit(string formId)
        {
            if (string.IsNullOrEmpty(formId))
            {
                return BadRequest("FormId is required.");
            }
            var form = await _formAPIService.GetFormAsync(formId);
            ViewBag.FormId = formId;
            return View("EditForm", form);
        }

        [HttpPost("form/{formId}/edit")]
        public async Task<IActionResult> Edit(FormModel updatedForm)
        {
            var form = await _formAPIService.GetFormAsync(updatedForm.FormId);
            form.StartPage = updatedForm.StartPage;
            form.Title = updatedForm.Title;
            form.Description = updatedForm.Description;
            form.Submission = updatedForm.Submission;
            await _formAPIService.UpdateFormAsync(form);
            return RedirectToAction("Index", new { formId = updatedForm.FormId });
        }

        [HttpGet("form/{formId}/addPage")]
        public IActionResult AddPage(string formId)
        {
            if (string.IsNullOrEmpty(formId))
            {
                return BadRequest("FormId is required.");
            }
            ViewBag.FormId = formId;
            return View("AddPage");
        }

        [HttpPost("form/{formId}/addPage")]
        public async Task<IActionResult> AddPage(string formId, Page newPage)
        {
            if (string.IsNullOrEmpty(formId))
            {
                return BadRequest("FormId is required.");
            }
            if (string.IsNullOrEmpty(newPage.PageId) || string.IsNullOrEmpty(newPage.Title) || string.IsNullOrEmpty(newPage.PageType) || string.IsNullOrEmpty(newPage.NextPageId))
            {
                ModelState.AddModelError(string.Empty, "All fields are required.");
                ViewBag.FormId = formId;
                return View("AddPage", newPage);
            }
            var form = await _formAPIService.GetFormAsync(formId);
            if (form.Pages == null)
            {
                form.Pages = new List<Page>();
            }
            form.Pages.Add(newPage);
            form.TotalPages = form.Pages.Count;
            await _formAPIService.UpdateFormAsync(form);
            return RedirectToAction("Edit", new { formId });
        }

        [HttpGet("form/{formId}/removePage/{pageId}")]
        public async Task<IActionResult> RemovePage(string formId, string pageId)
        {
            if (string.IsNullOrEmpty(formId) || string.IsNullOrEmpty(pageId))
            {
                return BadRequest("FormId and PageId are required.");
            }
            var form = await _formAPIService.GetFormAsync(formId);
            var page = form.Pages.FirstOrDefault(p => p.PageId == pageId);
            if (page != null)
            {
                form.Pages.Remove(page);
                form.TotalPages = form.Pages.Count;
                await _formAPIService.UpdateFormAsync(form);
            }
            return RedirectToAction("Edit", new { formId });
        }

        [HttpGet("form/{formId}/editPage/{pageId}")]
        public async Task<IActionResult> EditPage(string formId, string pageId)
        {
            if (string.IsNullOrEmpty(formId) || string.IsNullOrEmpty(pageId))
            {
                return BadRequest("FormId and PageId are required.");
            }
            var form = await _formAPIService.GetFormAsync(formId);
            var page = form.Pages.FirstOrDefault(p => p.PageId == pageId);
            if (page == null)
            {
                return NotFound();
            }
            ViewBag.FormId = formId;
            ViewBag.PageId = pageId;
            return View("EditPage", page);
        }

        [HttpPost("form/{formId}/editPage/{pageId}")]
        public async Task<IActionResult> EditPage(string formId, string pageId, Page updatedPage)
        {
            if (string.IsNullOrEmpty(formId) || string.IsNullOrEmpty(pageId))
            {
                return BadRequest("FormId and PageId are required.");
            }
            var form = await _formAPIService.GetFormAsync(formId);
            var page = form.Pages.FirstOrDefault(p => p.PageId == pageId);
            if (page == null)
            {
                return NotFound();
            }
            page.Title = updatedPage.Title;
            page.PageType = updatedPage.PageType;
            page.NextPageId = updatedPage.NextPageId;
            await _formAPIService.UpdateFormAsync(form);
            return RedirectToAction("Edit", new { formId });
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

        [HttpGet("form/{formId}/{page}/{repeatIndex?}")]
        public async Task<IActionResult> ShowPage(string formId, string page, int repeatIndex = -1)
        {   
            if (string.IsNullOrEmpty(formId) || string.IsNullOrEmpty(page))
            {
                return BadRequest("FormId and Page are required.");
            }
            var formModel = await _formAPIService.GetFormAsync(formId);
            if (formModel == null) return NotFound();

            var data = await _formAPIService.GetFormDataAsync(FormSessionHelper.GetApplicantId(HttpContext.Session));

            var errors = new Dictionary<string, List<string>>();
            if (Convert.ToBoolean(TempData["ReturnWIthErrors"]) == true)
            {
                errors = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(TempData["Errors"].ToString());
            }

            var result = await _formPresenter.HandlePage(page, formModel, data, errors, repeatIndex);
            if (result == null) return NotFound();

            ViewBag.CurrentPage = result.CurrentPage;
            ViewBag.TotalPages = result.TotalPages;
            ViewBag.FormId = formId;
            ViewBag.PreviousPageId = result.PreviousPage;
            ViewBag.Errors = result.Errors;

            return View(result.NextAction, result.PageModel);
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
            if (currentPage == null) 
            {
                throw new ArgumentException($"Page {pageId} not found in form {formId}");    
            }

            var formData = _formHelper.GetSubmittedPageData(currentPage, Request.Form.ToDictionary(x => x.Key, x => x.Value.ToString()));
            
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

            if (result.Errors != null && result.Errors.Any())
            {
                TempData["ReturnWIthErrors"] = true;
                TempData["Errors"] = JsonConvert.SerializeObject(result.Errors);

                return RedirectToAction(result.NextAction, new { formId = formModel.FormId, page = result.NextPage, repeatIndex = processResult.RepeatIndex });
            }

            var nextPage = formModel.Pages.Find(p => p.PageId == result.NextPage);
            if (nextPage == null) 
            {
                throw new ArgumentException($"Page {pageId} not found in form {formId}");    
            }

            if (nextPage.Repeating)
            {
                return RedirectToAction(result.NextAction, new { formId = formModel.FormId, page = result.NextPage, repeatIndex = processResult.RepeatIndex });
            }
            else 
            {
                return RedirectToAction(result.NextAction, new { formId = formModel.FormId, page = result.NextPage });
            }
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
            return View(result.NextAction, formModel);
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
            return View(result.NextAction, result.PageModel);
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

            return View(result.NextAction, result.ApplicationId);
        }
    }
}