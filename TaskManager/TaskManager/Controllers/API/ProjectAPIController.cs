using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using TaskManager.Services.Interfaces;
using System.Net;
using TaskManager.Models.DTOs;
using Microsoft.AspNetCore.Http;
using System;

namespace TaskManager.Controllers.API
{
    [ApiController]
    [Route("/api/users/me/projects/{projectId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProjectAPIController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectAPIController( IProjectService projectService )
        {
            _projectService = projectService;
        }

        [HttpDelete]
        public ActionResult DeleteProject(Guid projectId)
        {
            var allProjects = _projectService.GetAllFor(User.Identity.Name);

            var possibleResponse = CreateResponseIfRequestIsNotOK(projectId, "delete");
            if (possibleResponse != null)
            {
                return possibleResponse;
            }

            try
            {
                _projectService.Delete(projectId);
                return NoContent();
            } 
            catch(Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
                //TODO if this project will get a logger, this exception will need to be logged
            }
        }

        [HttpPut]
        public ActionResult ChangeProject([FromBody] ProjectWithCardsDTO changedProjectDTO, Guid projectId)
        {
            var changedProjectId = Guid.Parse(changedProjectDTO.Id);
            if (!changedProjectId.Equals(projectId)) 
            {
                return BadRequest(new { message = "The Id of the project is unclear from the request" });
            }

            var possibleResponse = CreateResponseIfRequestIsNotOK(projectId, "change");
            if (possibleResponse != null)
            {
                return possibleResponse;
            }

            try
            {
                var changedProject = _projectService.TranslateToProject(changedProjectDTO);
                _projectService.Update(changedProject);
                return Ok();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
                //TODO if this project will get a logger, this exception will need to be logged
            }
        }

        private ActionResult CreateResponseIfRequestIsNotOK(Guid projectId, string requestedAction)
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