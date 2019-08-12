using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("/users/token")]
        [Authorize]
        public IActionResult Token()
        {
            var role = User.IsInRole("TodoAdmin") ? "TodoAdmin" : "TodoUser";
            ViewData["token"] = _tokenService.GenerateToken(User.Identity.Name, role, false);
            return View();
        }
    }
}