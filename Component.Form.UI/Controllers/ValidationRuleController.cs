using Microsoft.AspNetCore.Mvc;
using Component.Form.Model;
using System.Linq;
using System.Threading.Tasks;
using Component.Form.UI.Services;

namespace Component.Form.UI.Controllers
{
    public class ValidationRuleController : Controller
    {
        private readonly FormAPIService _formAPIService;

        public ValidationRuleController(FormAPIService formAPIService)
        {
            _formAPIService = formAPIService;
        }

        [HttpGet("validationRule/{formId}/{pageId}/{componentName}")]
        public async Task<IActionResult> Index(string formId, string pageId, string componentName)
        {
            var form = await _formAPIService.GetFormAsync(formId);
            var page = form.Pages.FirstOrDefault(p => p.PageId == pageId);
            if (page == null)
            {
                return NotFound();
            }
            var component = page.Components.FirstOrDefault(c => c.Name == componentName);
            if (component == null)
            {
                return NotFound();
            }
            ViewBag.FormId = formId;
            ViewBag.PageId = pageId;
            ViewBag.ComponentName = componentName;
            return View("ListValidationRules", component.ValidationRules);
        }

        [HttpGet("validationRule/{formId}/{pageId}/{componentName}/add")]
        public IActionResult Add(string formId, string pageId, string componentName)
        {
            ViewBag.FormId = formId;
            ViewBag.PageId = pageId;
            ViewBag.ComponentName = componentName;
            return View("AddValidationRule");
        }

        [HttpPost("validationRule/{formId}/{pageId}/{componentName}/add")]
        public async Task<IActionResult> Add(string formId, string pageId, string componentName, ValidationRule validationRule)
        {
            if (string.IsNullOrEmpty(validationRule.Expression) || string.IsNullOrEmpty(validationRule.ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, "Expression and ErrorMessage are required fields.");
                ViewBag.FormId = formId;
                ViewBag.PageId = pageId;
                ViewBag.ComponentName = componentName;
                return View("AddValidationRule", validationRule);
            }
            var form = await _formAPIService.GetFormAsync(formId);
            var page = form.Pages.FirstOrDefault(p => p.PageId == pageId);
            if (page == null)
            {
                return NotFound();
            }
            var component = page.Components.FirstOrDefault(c => c.Name == componentName);
            if (component == null)
            {
                return NotFound();
            }
            validationRule.Id = Guid.NewGuid().ToString();
            if (component.ValidationRules == null)
            {
                component.ValidationRules = new List<ValidationRule>();
            }
            component.ValidationRules.Add(validationRule);
            await _formAPIService.UpdateFormAsync(form);
            return RedirectToAction("Index", new { formId, pageId, componentName });
        }

        [HttpGet("validationRule/{formId}/{pageId}/{componentName}/edit/{id}")]
        public async Task<IActionResult> Edit(string formId, string pageId, string componentName, string id)
        {
            var form = await _formAPIService.GetFormAsync(formId);
            var page = form.Pages.FirstOrDefault(p => p.PageId == pageId);
            if (page == null)
            {
                return NotFound();
            }
            var component = page.Components.FirstOrDefault(c => c.Name == componentName);
            if (component == null)
            {
                return NotFound();
            }
            var validationRule = component.ValidationRules.FirstOrDefault(v => v.Id == id);
            if (validationRule == null)
            {
                return NotFound();
            }
            ViewBag.FormId = formId;
            ViewBag.PageId = pageId;
            ViewBag.ComponentName = componentName;
            return View("EditValidationRule", validationRule);
        }

        [HttpPost("validationRule/{formId}/{pageId}/{componentName}/edit/{id}")]
        public async Task<IActionResult> Edit(string formId, string pageId, string componentName, string id, ValidationRule updatedValidationRule)
        {
            if (string.IsNullOrEmpty(updatedValidationRule.Expression) || string.IsNullOrEmpty(updatedValidationRule.ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, "Expression and ErrorMessage are required fields.");
                ViewBag.FormId = formId;
                ViewBag.PageId = pageId;
                ViewBag.ComponentName = componentName;
                return View("EditValidationRule", updatedValidationRule);
            }
            var form = await _formAPIService.GetFormAsync(formId);
            var page = form.Pages.FirstOrDefault(p => p.PageId == pageId);
            if (page == null)
            {
                return NotFound();
            }
            var component = page.Components.FirstOrDefault(c => c.Name == componentName);
            if (component == null)
            {
                return NotFound();
            }
            var validationRule = component.ValidationRules.FirstOrDefault(v => v.Id == id);
            if (validationRule == null)
            {
                return NotFound();
            }
            validationRule.Expression = updatedValidationRule.Expression;
            validationRule.ErrorMessage = updatedValidationRule.ErrorMessage;
            await _formAPIService.UpdateFormAsync(form);
            return RedirectToAction("Index", new { formId, pageId, componentName });
        }

        [HttpGet("validationRule/{formId}/{pageId}/{componentName}/delete/{id}")]
        public async Task<IActionResult> Delete(string formId, string pageId, string componentName, string id)
        {
            var form = await _formAPIService.GetFormAsync(formId);
            var page = form.Pages.FirstOrDefault(p => p.PageId == pageId);
            if (page == null)
            {
                return NotFound();
            }
            var component = page.Components.FirstOrDefault(c => c.Name == componentName);
            if (component == null)
            {
                return NotFound();
            }
            var validationRule = component.ValidationRules.FirstOrDefault(v => v.Id == id);
            if (validationRule == null)
            {
                return NotFound();
            }
            component.ValidationRules.Remove(validationRule);
            await _formAPIService.UpdateFormAsync(form);
            return RedirectToAction("Index", new { formId, pageId, componentName });
        }
    }
}