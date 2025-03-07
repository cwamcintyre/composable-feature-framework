using Microsoft.AspNetCore.Mvc;
using Component.Form.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Component.Form.UI.Services;
using Microsoft.Extensions.Options;

namespace Component.Form.UI.Controllers
{
    public class ConditionController : Controller
    {
        private readonly FormAPIService _formAPIService;

        public ConditionController(FormAPIService formAPIService)
        {
            _formAPIService = formAPIService;
        }

        [HttpGet("condition/{formId}/{pageId}")]
        public async Task<IActionResult> Index(string formId, string pageId)
        {
            var form = await _formAPIService.GetFormAsync(formId);
            var page = form.Pages.FirstOrDefault(p => p.PageId == pageId);
            if (page == null)
            {
                return NotFound();
            }
            ViewBag.FormId = formId;
            ViewBag.PageId = pageId;
            return View("ListConditions", page.Conditions);
        }

        [HttpGet("condition/{formId}/{pageId}/add")]
        public IActionResult Add(string formId, string pageId)
        {
            ViewBag.FormId = formId;
            ViewBag.PageId = pageId;
            return View("AddCondition");
        }

        [HttpPost("condition/{formId}/{pageId}/add")]
        public async Task<IActionResult> Add(string formId, string pageId, Condition condition)
        {
            if (string.IsNullOrEmpty(condition.Expression) || string.IsNullOrEmpty(condition.NextPageId))
            {
                ModelState.AddModelError(string.Empty, "Expression and NextPageId are required fields.");
                ViewBag.FormId = formId;
                ViewBag.PageId = pageId;
                return View("AddCondition", condition);
            }
            var form = await _formAPIService.GetFormAsync(formId);
            var page = form.Pages.FirstOrDefault(p => p.PageId == pageId);
            if (page == null)
            {
                return NotFound();
            }
            condition.Id = Guid.NewGuid().ToString();
            page.Conditions.Add(condition);
            await _formAPIService.UpdateFormAsync(form);
            return RedirectToAction("Index", new { formId, pageId });
        }

        [HttpGet("condition/{formId}/{pageId}/edit/{id}")]
        public async Task<IActionResult> Edit(string formId, string pageId, string id)
        {
            var form = await _formAPIService.GetFormAsync(formId);
            var page = form.Pages.FirstOrDefault(p => p.PageId == pageId);
            if (page == null)
            {
                return NotFound();
            }
            var condition = page.Conditions.FirstOrDefault(c => c.Id == id);
            if (condition == null)
            {
                return NotFound();
            }
            ViewBag.FormId = formId;
            ViewBag.PageId = pageId;
            return View("EditCondition", condition);
        }

        [HttpPost("condition/{formId}/{pageId}/edit/{id}")]
        public async Task<IActionResult> Edit(string formId, string pageId, string id, Condition updatedCondition)
        {
            if (string.IsNullOrEmpty(updatedCondition.Expression) || string.IsNullOrEmpty(updatedCondition.NextPageId))
            {
                ModelState.AddModelError(string.Empty, "Expression and NextPageId are required fields.");
                ViewBag.FormId = formId;
                ViewBag.PageId = pageId;
                return View("EditCondition", updatedCondition);
            }
            var form = await _formAPIService.GetFormAsync(formId);
            var page = form.Pages.FirstOrDefault(p => p.PageId == pageId);
            if (page == null)
            {
                return NotFound();
            }
            var condition = page.Conditions.FirstOrDefault(c => c.Id == id);
            if (condition == null)
            {
                return NotFound();
            }
            condition.Expression = updatedCondition.Expression;
            condition.NextPageId = updatedCondition.NextPageId;
            await _formAPIService.UpdateFormAsync(form);
            return RedirectToAction("Index", new { formId, pageId });
        }

        [HttpGet("condition/{formId}/{pageId}/delete/{id}")]
        public async Task<IActionResult> Delete(string formId, string pageId, string id)
        {
            var form = await _formAPIService.GetFormAsync(formId);
            var page = form.Pages.FirstOrDefault(p => p.PageId == pageId);
            if (page == null)
            {
                return NotFound();
            }
            var condition = page.Conditions.FirstOrDefault(c => c.Id == id);
            if (condition == null)
            {
                return NotFound();
            }
            page.Conditions.Remove(condition);
            await _formAPIService.UpdateFormAsync(form);
            return RedirectToAction("Index", new { formId, pageId });
        }
    }
}