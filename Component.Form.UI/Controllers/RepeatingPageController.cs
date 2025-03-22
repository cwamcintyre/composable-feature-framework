using Component.Form.UI.Helpers;
using Component.Form.UI.PageHandler;
using Component.Form.UI.Presenters;
using Component.Form.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace Component.Form.UI.Controllers
{
    public class RepeatingPageController : Controller
    {
        private readonly FormAPIService _formAPIService;
        private readonly IRepeatingPagePresenter _repeatPresenter;
        private readonly PageHandlerFactory _pageHanderFactory;
        
        public RepeatingPageController(
            IRepeatingPagePresenter repeatPresenter, 
            FormAPIService formAPIService, 
            PageHandlerFactory pageHanderFactory)
        {
            _formAPIService = formAPIService;
            _repeatPresenter = repeatPresenter;
            _pageHanderFactory = pageHanderFactory;
        }

        [HttpGet("form/{formId}/{pageId}/remove/{repeatIndex}")]
        public async Task<IActionResult> RemoveRepeatingSection(string formId, string pageId, int repeatIndex)
        {
            if (string.IsNullOrEmpty(formId) || string.IsNullOrEmpty(pageId))
            {
                return BadRequest("FormId, Page and repeat index are required.");
            }

            var formModel = await _formAPIService.GetFormAsync(formId);
            if (formModel == null) return NotFound();

            var currentPage = formModel.Pages.Find(p => p.PageId == pageId);
            if (currentPage == null) return NotFound();

            var pageHandler = _pageHanderFactory.GetFor(currentPage.PageType);
            if (pageHandler == null) return NotFound();

            var response = await _formAPIService.RemoveRepeatingSection(formId, pageId, FormSessionHelper.GetApplicantId(HttpContext.Session), repeatIndex);

            var result = await _repeatPresenter.HandleRemoveAction();

            ViewBag.FormId = formId;

            return RedirectToAction(result.Action, result.Controller, new { formId = formId });
        }


        [HttpGet("form/{formId}/{pageId}/add")]
        public async Task<IActionResult> AddRepeatingSection(string formId, string pageId) 
        {
            if (string.IsNullOrEmpty(formId) || string.IsNullOrEmpty(pageId))
            {
                return BadRequest("FormId and Page are required.");
            }
            
            var formModel = await _formAPIService.GetFormAsync(formId);
            if (formModel == null) return NotFound();

            var currentPage = formModel.Pages.Find(p => p.PageId == pageId);
            if (currentPage == null) return NotFound();

            var pageHandler = _pageHanderFactory.GetFor(currentPage.PageType);
            if (pageHandler == null) return NotFound();

            var response = await _formAPIService.AddRepeatingSection(formId, pageId, FormSessionHelper.GetApplicantId(HttpContext.Session));

            var result = await _repeatPresenter.HandleAddAction();

            ViewBag.FormId = formId;

            return RedirectToAction(result.Action, result.Controller, new { formId = formId, page = pageId, extraData = $"{response.NewRepeatIndex}-{response.StartPageId}" });
        }
    }
}
