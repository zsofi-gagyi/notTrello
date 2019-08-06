using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoWithDatabase.Services.Interfaces;

namespace TodoWithDatabase.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly IAssigneeService _assigneeService;

        public ProjectController(IProjectService projectService, IAssigneeService assigneeService)
        {
            _projectService = projectService;
            _assigneeService = assigneeService;
        }

        [HttpGet("/users/projects/{Id}")]
        [Authorize]
        public IActionResult ProjectWithCards([FromRoute(Name = "Id")]string projectId)
        {
            string name = User.Identity.Name;
            var isAllowed = _projectService.userCollaboratesOnProject(name, projectId); // this will become middleware!

            if (isAllowed)
            {
                ViewData["project"] = _projectService.GetWithCards(projectId);
            }

            return View();
        }
    }
}