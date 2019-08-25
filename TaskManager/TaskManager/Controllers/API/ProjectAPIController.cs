using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using TodoWithDatabase.Services.Interfaces;
using System.Net;
using TodoWithDatabase.Models.DTOs;

namespace TodoWithDatabase.Controllers.API
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProjectAPIController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectAPIController( IProjectService projectService )
        {
            _projectService = projectService;
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
        public ActionResult ChangeProject([FromBody] ProjectWithCardsDTO changedProjectDTO, string projectId)
        {
            if (!changedProjectDTO.Id.Equals(projectId))
            {
                return BadRequest(new { message = "The Id of the project is unclear from the request" });
            }

            var possibleResponse = CreateResponseIfRequestIsNotOK(projectId, "change");
            if (possibleResponse != null)
            {
                return possibleResponse;
            }

            var changedProject = _projectService.TranslateToProject(changedProjectDTO);
            _projectService.Update(changedProject);
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
                return BadRequest(new { message = "The project is shared with other users. Only solo projects can be " 
                    + requestedAction + "d using this method" });
            }

            return null;
        }
    }
}