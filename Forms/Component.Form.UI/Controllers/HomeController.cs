using Microsoft.AspNetCore.Mvc;

namespace Component.Form.UI.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Error()
        {
            return View();
        }
    }
}
