using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
                                               
        [HttpGet("/users")]
        [Authorize]
        public IActionResult UserMainPage()
        {
            ViewData["projects"] = _projectService.GetAllFor(User.Identity.Name);
            return View();
        }
    }
}
