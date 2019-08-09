using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace TodoWithDatabase.Controllers
{
    public class PublicController : Controller
    {
        [HttpGet("/")]
        [AllowAnonymous] //TODO make this a default in a more elegant way
        public IActionResult RedirectToMain()
        {
            return Redirect("/main");
        }
    }
}
