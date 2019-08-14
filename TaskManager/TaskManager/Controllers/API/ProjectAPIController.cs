using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using TodoWithDatabase.Services.Interfaces;
using TodoWithDatabase.Models.DTO;
using Microsoft.AspNetCore.Identity;
using TodoWithDatabase.Models.DAOs;
using System.Net;

namespace TodoWithDatabase.Controllers.API
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProjectAPIController : ControllerBase
    {
        private readonly IAssigneeService _assigneeService;
        private readonly IProjectService _projectService;
        private readonly UserManager<Assignee> _userManager;

        public ProjectAPIController(IAssigneeService assigneeService,IProjectService projectService, UserManager<Assignee> userManager)
        {
            _assigneeService = assigneeService;
            _projectService = projectService;
            _userManager = userManager;
        }

        [HttpDelete("/api/users/me/projects/{projectId}")]
        public ActionResult<AssigneeWithCardsDTO> GetAssigneeProjectFor(string projectId)
        {
            var project = _projectService.GetWithAssigneeProjects(projectId);
            var userIsEntitledToDelete = _projectService.UserIsCollaboratingOnProject(User.Identity.Name, projectId);

            if (!userIsEntitledToDelete)
            {
                return StatusCode((int)HttpStatusCode.Forbidden, new { message = "Users can delete only their own projects." }); 
            }

            if (project.AssigneeProjects.Count == 1)
            {
                _projectService.Delete(projectId);
                return NoContent();
            }

            return BadRequest(new { message = "The project is shared with other users. Only solo projects can be deleted using this method" });
        }
    }
}