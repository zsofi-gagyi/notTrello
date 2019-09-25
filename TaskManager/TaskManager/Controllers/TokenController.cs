using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TaskManager.Models.DAOs;
using TaskManager.Services.Interfaces;

namespace TaskManager.Controllers
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
        public async Task<IActionResult> Token()
        {
            try
            {
                var userName = User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            var userId = user.Id;
            var role = User.IsInRole("TodoAdmin") ? "TodoAdmin" : "TodoUser";
                try
                {
                    ViewData["token"] = _tokenService.GenerateToken(userId, userName, role);
                }
                catch (Exception e)
                {
                    ViewData["token"] = e.Message + " when generating token";
                }
            }
            catch (Exception e)
            {
                ViewData["token"] = e.Message + " during preparation";
            }
            return View();
        }
    }
}