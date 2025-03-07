using Microsoft.AspNetCore.Mvc;
using Component.Form.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Component.Form.UI.Services;
using Microsoft.Extensions.Options;

namespace Component.Form.UI.Controllers
{
    public class OptionController : Controller
    {
        private readonly FormAPIService _formAPIService;

        public OptionController(FormAPIService formAPIService)
        {
            _formAPIService = formAPIService;
        }

        [HttpGet("option/{formId}/{pageId}/{componentName}")]
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
            return View("ListOptions", component.Options.FromDictionary());
        }

        [HttpGet("option/{formId}/{pageId}/{componentName}/add")]
        public IActionResult Add(string formId, string pageId, string componentName)
        {
            ViewBag.FormId = formId;
            ViewBag.PageId = pageId;
            ViewBag.ComponentName = componentName;
            return View("AddOption");
        }

        [HttpPost("option/{formId}/{pageId}/{componentName}/add")]
        public async Task<IActionResult> Add(string formId, string pageId, string componentName, Option option)
        {
            if (string.IsNullOrEmpty(option.Key) || string.IsNullOrEmpty(option.Value))
            {
                ModelState.AddModelError(string.Empty, "Key and Value are required fields.");
                ViewBag.FormId = formId;
                ViewBag.PageId = pageId;
                ViewBag.ComponentName = componentName;
                return View("AddOption", option);
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
            if (component.Options == null)
            {
                component.Options = new Dictionary<string, string>();
            }
            component.Options.Add(option.Key, option.Value);
            await _formAPIService.UpdateFormAsync(form);
            return RedirectToAction("Index", new { formId, pageId, componentName });
        }

        [HttpGet("option/{formId}/{pageId}/{componentName}/edit/{key}")]
        public async Task<IActionResult> Edit(string formId, string pageId, string componentName, string key)
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
            if (!component.Options.ContainsKey(key))
            {
                return NotFound();
            }
            ViewBag.FormId = formId;
            ViewBag.PageId = pageId;
            ViewBag.ComponentName = componentName;
            return View("EditOption", component.Options[key]);
        }

        [HttpPost("option/{formId}/{pageId}/{componentName}/edit/{key}")]
        public async Task<IActionResult> Edit(string formId, string pageId, string componentName, string key, Option updatedOption)
        {
            if (string.IsNullOrEmpty(updatedOption.Key) || string.IsNullOrEmpty(updatedOption.Value))
            {
                ModelState.AddModelError(string.Empty, "Key and Value are required fields.");
                ViewBag.FormId = formId;
                ViewBag.PageId = pageId;
                ViewBag.ComponentName = componentName;
                return View("EditOption", updatedOption);
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
            if (!component.Options.ContainsKey(key))
            {
                return NotFound();
            }
            component.Options.Remove(key);
            component.Options.Add(updatedOption.Key, updatedOption.Value);
            await _formAPIService.UpdateFormAsync(form);
            return RedirectToAction("Index", new { formId, pageId, componentName });
        }

        [HttpGet("option/{formId}/{pageId}/{componentName}/delete/{key}")]
        public async Task<IActionResult> Delete(string formId, string pageId, string componentName, string key)
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
            if (!component.Options.ContainsKey(key))
            {
                return NotFound();
            }
            component.Options.Remove(key);
            await _formAPIService.UpdateFormAsync(form);
            return RedirectToAction("Index", new { formId, pageId, componentName });
        }
    }
}