using System;
using Microsoft.AspNetCore.Mvc;

namespace Component.Form.UI.Controllers;

public class FormAdminController : Controller
{
    // [HttpGet("form/{formId}/list")]
    // public async Task<IActionResult> Index(string formId)
    // {
    //     if (string.IsNullOrEmpty(formId))
    //     {
    //         return BadRequest("FormId is required.");
    //     }
    //     var form = await _formAPIService.GetFormAsync(formId);
    //     ViewBag.FormId = formId;
    //     return View("ListForm", form);
    // }

    // [HttpGet("form/{formId}/edit")]
    // public async Task<IActionResult> Edit(string formId)
    // {
    //     if (string.IsNullOrEmpty(formId))
    //     {
    //         return BadRequest("FormId is required.");
    //     }
    //     var form = await _formAPIService.GetFormAsync(formId);
    //     ViewBag.FormId = formId;
    //     return View("EditForm", form);
    // }

    // [HttpPost("form/{formId}/edit")]
    // public async Task<IActionResult> Edit(FormModel updatedForm)
    // {
    //     var form = await _formAPIService.GetFormAsync(updatedForm.FormId);
    //     form.StartPage = updatedForm.StartPage;
    //     form.Title = updatedForm.Title;
    //     form.Description = updatedForm.Description;
    //     form.Submission = updatedForm.Submission;
    //     await _formAPIService.UpdateFormAsync(form);
    //     return RedirectToAction("Index", new { formId = updatedForm.FormId });
    // }

    // [HttpGet("form/{formId}/addPage")]
    // public IActionResult AddPage(string formId)
    // {
    //     if (string.IsNullOrEmpty(formId))
    //     {
    //         return BadRequest("FormId is required.");
    //     }
    //     ViewBag.FormId = formId;
    //     return View("AddPage");
    // }

    // [HttpPost("form/{formId}/addPage")]
    // public async Task<IActionResult> AddPage(string formId, Page newPage)
    // {
    //     if (string.IsNullOrEmpty(formId))
    //     {
    //         return BadRequest("FormId is required.");
    //     }
    //     if (string.IsNullOrEmpty(newPage.PageId) || string.IsNullOrEmpty(newPage.Title) || string.IsNullOrEmpty(newPage.PageType) || string.IsNullOrEmpty(newPage.NextPageId))
    //     {
    //         ModelState.AddModelError(string.Empty, "All fields are required.");
    //         ViewBag.FormId = formId;
    //         return View("AddPage", newPage);
    //     }
    //     var form = await _formAPIService.GetFormAsync(formId);
    //     if (form.Pages == null)
    //     {
    //         form.Pages = new List<Page>();
    //     }
    //     form.Pages.Add(newPage);
    //     form.TotalPages = form.Pages.Count;
    //     await _formAPIService.UpdateFormAsync(form);
    //     return RedirectToAction("Edit", new { formId });
    // }

    // [HttpGet("form/{formId}/removePage/{pageId}")]
    // public async Task<IActionResult> RemovePage(string formId, string pageId)
    // {
    //     if (string.IsNullOrEmpty(formId) || string.IsNullOrEmpty(pageId))
    //     {
    //         return BadRequest("FormId and PageId are required.");
    //     }
    //     var form = await _formAPIService.GetFormAsync(formId);
    //     var page = form.Pages.FirstOrDefault(p => p.PageId == pageId);
    //     if (page != null)
    //     {
    //         form.Pages.Remove(page);
    //         form.TotalPages = form.Pages.Count;
    //         await _formAPIService.UpdateFormAsync(form);
    //     }
    //     return RedirectToAction("Edit", new { formId });
    // }

    // [HttpGet("form/{formId}/editPage/{pageId}")]
    // public async Task<IActionResult> EditPage(string formId, string pageId)
    // {
    //     if (string.IsNullOrEmpty(formId) || string.IsNullOrEmpty(pageId))
    //     {
    //         return BadRequest("FormId and PageId are required.");
    //     }
    //     var form = await _formAPIService.GetFormAsync(formId);
    //     var page = form.Pages.FirstOrDefault(p => p.PageId == pageId);
    //     if (page == null)
    //     {
    //         return NotFound();
    //     }
    //     ViewBag.FormId = formId;
    //     ViewBag.PageId = pageId;
    //     return View("EditPage", page);
    // }

    // [HttpPost("form/{formId}/editPage/{pageId}")]
    // public async Task<IActionResult> EditPage(string formId, string pageId, Page updatedPage)
    // {
    //     if (string.IsNullOrEmpty(formId) || string.IsNullOrEmpty(pageId))
    //     {
    //         return BadRequest("FormId and PageId are required.");
    //     }
    //     var form = await _formAPIService.GetFormAsync(formId);
    //     var page = form.Pages.FirstOrDefault(p => p.PageId == pageId);
    //     if (page == null)
    //     {
    //         return NotFound();
    //     }
    //     page.Title = updatedPage.Title;
    //     page.PageType = updatedPage.PageType;
    //     page.NextPageId = updatedPage.NextPageId;
    //     await _formAPIService.UpdateFormAsync(form);
    //     return RedirectToAction("Edit", new { formId });
    // }
}
