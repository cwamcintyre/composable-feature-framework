using Microsoft.AspNetCore.Mvc;
using Component.Form.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Component.Form.UI.Services;

namespace Component.Form.UI.Controllers
{
    public class ComponentController : Controller
    {
        private readonly FormAPIService _formAPIService;

        public ComponentController(FormAPIService formAPIService)
        {
            _formAPIService = formAPIService;
        }

        [HttpGet("component/{formId}/{pageId}")]
        public async Task<IActionResult> Index(string formId, string pageId)
        {
            var form = await _formAPIService.GetFormAsync(formId);
            var page = form.Pages.FirstOrDefault(p => p.PageId == pageId);
            if (page == null)
            {
                return NotFound();
            }
            return View("ListComponents", page.Components);
        }

        [HttpGet("component/{formId}/{pageId}/add")]
        public IActionResult Add(string formId, string pageId)
        {
            ViewBag.FormId = formId;
            ViewBag.PageId = pageId;
            return View("AddComponent");
        }

        [HttpPost("component/{formId}/{pageId}/add")]
        public async Task<IActionResult> Add(string formId, string pageId, Component.Form.Model.Component component)
        {
            if (string.IsNullOrEmpty(component.Type) || string.IsNullOrEmpty(component.Name) || string.IsNullOrEmpty(component.Label))
            {
                ModelState.AddModelError(string.Empty, "Type, Name, and Label are required fields.");
                ViewBag.FormId = formId;
                ViewBag.PageId = pageId;
                return View("AddComponent", component);
            }
            var form = await _formAPIService.GetFormAsync(formId);
            var page = form.Pages.FirstOrDefault(p => p.PageId == pageId);
            if (page == null)
            {
                return NotFound();
            }
            if (page.Components == null)
            {
                page.Components = new List<Component.Form.Model.Component>();
            }
            page.Components.Add(component);
            await _formAPIService.UpdateFormAsync(form);
            return RedirectToAction("Index", new { formId, pageId });
        }

        [HttpGet("component/{formId}/{pageId}/edit/{name}")]
        public async Task<IActionResult> Edit(string formId, string pageId, string name)
        {
            var form = await _formAPIService.GetFormAsync(formId);
            var page = form.Pages.FirstOrDefault(p => p.PageId == pageId);
            if (page == null)
            {
                return NotFound();
            }
            var component = page.Components.FirstOrDefault(c => c.Name == name);
            if (component == null)
            {
                return NotFound();
            }
            ViewBag.FormId = formId;
            ViewBag.PageId = pageId;
            return View("EditComponent", component);
        }

        [HttpPost("component/{formId}/{pageId}/edit/{name}")]
        public async Task<IActionResult> Edit(string formId, string pageId, string name, Component.Form.Model.Component updatedComponent)
        {
            if (string.IsNullOrEmpty(updatedComponent.Type) || string.IsNullOrEmpty(updatedComponent.Name) || string.IsNullOrEmpty(updatedComponent.Label))
            {
                ModelState.AddModelError(string.Empty, "Type, Name, and Label are required fields.");
                ViewBag.FormId = formId;
                ViewBag.PageId = pageId;
                return View("EditComponent", updatedComponent);
            }
            var form = await _formAPIService.GetFormAsync(formId);
            var page = form.Pages.FirstOrDefault(p => p.PageId == pageId);
            if (page == null)
            {
                return NotFound();
            }
            var component = page.Components.FirstOrDefault(c => c.Name == name);
            if (component == null)
            {
                return NotFound();
            }
            component.QuestionId = updatedComponent.QuestionId;
            component.Type = updatedComponent.Type;
            component.Label = updatedComponent.Label;
            component.Name = updatedComponent.Name;
            component.LabelIsPageTitle = updatedComponent.LabelIsPageTitle;
            component.Hint = updatedComponent.Hint;
            component.Optional = updatedComponent.Optional;
            component.Content = updatedComponent.Content;
            await _formAPIService.UpdateFormAsync(form);
            return RedirectToAction("Index", new { formId, pageId });
        }

        [HttpGet("component/{formId}/{pageId}/delete/{name}")]
        public async Task<IActionResult> Delete(string formId, string pageId, string name)
        {
            var form = await _formAPIService.GetFormAsync(formId);
            var page = form.Pages.FirstOrDefault(p => p.PageId == pageId);
            if (page == null)
            {
                return NotFound();
            }
            var component = page.Components.FirstOrDefault(c => c.Name == name);
            if (component == null)
            {
                return NotFound();
            }
            page.Components.Remove(component);
            await _formAPIService.UpdateFormAsync(form);
            return RedirectToAction("Index", new { formId, pageId });
        }
    }
}