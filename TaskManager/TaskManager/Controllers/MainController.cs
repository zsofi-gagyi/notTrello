using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Models.ViewModels;
using TaskManager.Services.Interfaces;

namespace TaskManager.Controllers
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
            var viewModel = new ProjectsViewModel();
            viewModel.projects =  _projectService.GetAllFor(User.Identity.Name);
            return View(viewModel);
        }
    } 

}
