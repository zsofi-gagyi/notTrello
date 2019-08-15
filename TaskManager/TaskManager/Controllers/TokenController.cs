using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TodoWithDatabase.Models.DAOs;
using TodoWithDatabase.Services.Interfaces;

namespace TodoWithDatabase.Controllers
{
    public class 
        TokenController : Controller
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<Assignee> _userManager;

        public TokenController(ITokenService tokenService, UserManager<Assignee> userManager)
        {
            _tokenService = tokenService;
            _userManager = userManager;
        }

        [HttpGet("/users/token")]
        [Authorize]
        public IActionResult Token()
        {
            var userName = User.Identity.Name;
            var userId = _userManager.FindByNameAsync(userName).Result.Id;
            var role = User.IsInRole("TodoAdmin") ? "TodoAdmin" : "TodoUser";
            ViewData["token"] = _tokenService.GenerateToken(userId, userName, role);
            return View();
        }
    }
}