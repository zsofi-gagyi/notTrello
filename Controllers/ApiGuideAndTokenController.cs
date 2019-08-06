using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoWithDatabase.Services;
using TodoWithDatabase.Services.Interfaces;

namespace TodoWithDatabase.Controllers
{
    public class ApiGuideAndTokenController : Controller
    {
        private readonly ITokenService _tokenService;

        public ApiGuideAndTokenController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpGet("/users/APIguide")]
        [Authorize(Roles = "TodoUser" + "," + "TodoAdmin")]
        public IActionResult APIguideForAssignees()
        {
            return View("Views/Web/APIguide.cshtml");
        }

        [HttpGet("/APIguide")]
        public IActionResult APIguide()
        {
            return View("Views/Web/APIguide.cshtml");
        }

        [HttpGet("/users/token")]
        [Authorize(Roles = "TodoUser" + "," + "TodoAdmin")]
        public IActionResult token()
        {
            var role = User.IsInRole("TodoAdmin") ? "TodoAdmin" : "TodoUser";
            ViewData["token"] = _tokenService.GenerateToken(User.Identity.Name, role, false);
            return View("Views/Web/Token.cshtml");
        }
    }
}