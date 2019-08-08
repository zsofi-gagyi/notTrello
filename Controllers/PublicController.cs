using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace TodoWithDatabase.Controllers
{
    public class PublicController : Controller
    {
        ICompositeViewEngine _viewEngine;

        public PublicController(ICompositeViewEngine viewEngine)
        {
            _viewEngine = viewEngine;
        }

        [HttpGet("/")]
        [AllowAnonymous]
        public IActionResult RedirectToMain()
        {
            return Redirect("/public/main");
        }

        [HttpGet("/public/{pageName}")]
        [AllowAnonymous]
        public IActionResult AllPublicPages(string pageName)
        {
            var result = _viewEngine.FindView(ControllerContext, pageName, false);
            if (result == null || result.View == null)
            {
                return NotFound();
            }
            return View(pageName);
        }
    }
}
