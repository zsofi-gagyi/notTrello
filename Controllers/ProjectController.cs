using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TodoWithDatabase.Models;
using TodoWithDatabase.Models.DAOs;
using TodoWithDatabase.Models.DAOs.JoinTables;
using TodoWithDatabase.Services.Interfaces;

namespace TodoWithDatabase.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly IAssigneeService _assigneeService;
        private readonly UserManager<Assignee> _userManager;

        public ProjectController(IProjectService projectService, IAssigneeService assigneeService, UserManager<Assignee> userManager)
        {
            _projectService = projectService;
            _assigneeService = assigneeService;
            _userManager = userManager;
        }

        [HttpGet("/users/projects/{Id}")]
        [Authorize]
        public IActionResult ProjectWithCards([FromRoute(Name = "Id")]string projectId)
        {
            string name = User.Identity.Name;
            var isAllowed = _projectService.userCollaboratesOnProject(name, projectId); // this will become middleware!

            if (isAllowed)
            {
                var project = _projectService.GetWithCards(projectId);
                ViewData.Add("project", project);
            }

            return View();
        }

        [HttpGet("/users/addProject")]
        [Authorize]
        public IActionResult AddProject()
        {
            var otherAssignees = _userManager.Users.Where(u => u.UserName != User.Identity.Name).ToList();
            ViewData.Add("otherAssignees", otherAssignees);
            return View();
        }

        [HttpPost("/users/addProject")]
        [Authorize]
        public IActionResult AddProject([FromForm] Project project, List<string> collaboratorIds)
        {
            _projectService.Save(project); //so it gets an ID generated in the database, which we will need for the next step

            var responsibles = new List<Assignee>();
            responsibles.Add(_userManager.GetUserAsync(User).Result);
            foreach (string id in collaboratorIds)
            {
                var collaborator = _userManager.FindByIdAsync(id).Result;
                responsibles.Add(collaborator);
            }

            foreach(Assignee responsible in responsibles)
            {
                project.AssigneeProjects.Add(new AssigneeProject(responsible, project));
            }

            _projectService.Update(project);
            return Redirect("https://www.learnrazorpages.com/razor-pages/model-binding");
        }
    }
}