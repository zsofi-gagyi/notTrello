using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoWithDatabase.Services.Interfaces;

namespace TodoWithDatabase.Controllers
{
    public class 
        TokenController : Controller
    {
        private readonly ITokenService _tokenService;

        public TokenController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpGet("/users/token")]
        [Authorize]
        public IActionResult Token()
        {
            var userId = User.FindFirst(c => c.Type.Equals("UserId")).Value;
            var userName = User.Identity.Name;
            var role = User.IsInRole("TodoAdmin") ? "TodoAdmin" : "TodoUser";
            ViewData["token"] = _tokenService.GenerateToken(userId, userName, role);
            return View();
        }
    }
}