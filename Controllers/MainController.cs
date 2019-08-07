using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TodoWithDatabase.Models;
using TodoWithDatabase.Services;
using TodoWithDatabase.Services.Interfaces;

namespace TodoWithDatabase.Controllers
{
    public class MainController : Controller
    {
        private readonly IProjectService _projectService;

        public MainController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet("/")]
        public IActionResult MainPage()
        {
            return Redirect("/mainPage.html");
        }
                                               
        [HttpGet("/users")]
        [Authorize]
        public IActionResult TodoList()
        {
            ViewData["projects"] = _projectService.GetAllFor(User.Identity.Name);
            return View();
        }
    }
}
