using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Models.DAOs;
using TaskManager.Models.DAOs.JoinTables;
using TaskManager.Models.ViewModels;
using TaskManager.Services.Interfaces;

namespace TaskManager.Controllers
{
    [Route("/users")]
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly UserManager<Assignee> _userManager;

        public ProjectController(IProjectService projectService, UserManager<Assignee> userManager)
        {
            _projectService = projectService;
            _userManager = userManager;
        }

        [HttpGet("projects/{Id}")]
        [Authorize]
        public IActionResult ProjectWithCards([FromRoute(Name = "Id")]Guid projectId)
        {
            var viewModel = new ProjectViewModel();
            viewModel.project = _projectService.GetWithCards(projectId);
            return View(viewModel);
        }

        [HttpGet("addProject")]
        [Authorize]
        public IActionResult AddProject()
        {
            var viewModel = new AssigneesViewModel();
            viewModel.assignees =  _userManager.Users.Where(u => u.UserName != User.Identity.Name).ToList();
            return View(viewModel);
        }

        [HttpPost("addProject")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> AddProject([FromForm] Project project, List<string> collaboratorIds)
        {
            _projectService.Save(project); 

            var responsibles = new List<Assignee>() { await _userManager.GetUserAsync(User) };
            foreach (string id in collaboratorIds)
            {
                                                                 //TODO solve this with a single trip to the database
                var collaborator = await _userManager.FindByIdAsync(id);
                responsibles.Add(collaborator);
            }

            foreach (Assignee responsible in responsibles)
            {
                                                                 //TODO solve this with a single trip to the database
                project.AssigneeProjects.Add(new AssigneeProject(responsible, project));
            }

            _projectService.Update(project);
            return Redirect("/users/projects/" + project.Id);
        }
    }
}