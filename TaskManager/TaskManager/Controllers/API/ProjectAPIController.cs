using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using TodoWithDatabase.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using TodoWithDatabase.Models.DAOs;
using System.Net;
using TodoWithDatabase.Models.DTOs;

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
        public ActionResult DeleteProject(string projectId)
        {
            var possibleResponse = CreateResponseIfRequestIsNotOK(projectId, "delete");
            if (possibleResponse != null)
            {
                return possibleResponse;
            }
            
            _projectService.Delete(projectId);
            return NoContent();
        }

        [HttpPut("/api/users/me/projects/{projectId}")]
        public ActionResult ChangeProject([FromBody] ProjectWithCardsDTO changedProject, string projectId)
        {
            if (!changedProject.Id.Equals(projectId))
            {
                return BadRequest(new { message = "The Id of the project is unclear from the request" });
            }

            var possibleResponse = CreateResponseIfRequestIsNotOK(projectId, "change");
            if (possibleResponse != null)
            {
                return possibleResponse;
            }

            _projectService.TranslateAndUpdate(changedProject);
            return Ok();
        }

        private ActionResult CreateResponseIfRequestIsNotOK(string projectId, string requestedAction)
        {
            var project = _projectService.GetWithAssigneeProjects(projectId);
            var userIsEntitledToDelete = _projectService.UserIsCollaboratingOnProject(User.Identity.Name, projectId);

            if (!userIsEntitledToDelete)
            {
                return StatusCode((int)HttpStatusCode.Forbidden, new { message = "Users can " + requestedAction + " only their own projects." });
            }

            if (project.AssigneeProjects.Count != 1)
            {
                return BadRequest(new { message = "The project is shared with other users. Only solo projects can be " + requestedAction + "d using this method" });
            }

            return null;
        }
    }
}