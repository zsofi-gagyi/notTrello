using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TaskManager.Controllers
{
    public class DefaultController : Controller 
    {
        [HttpGet("/")]
        [AllowAnonymous] 
        public IActionResult RedirectFromBasicDomain() 
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/users");
            }
            else
            {
                return Redirect("/main");
            }
        }
    }
}
