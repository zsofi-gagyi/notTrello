using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TodoWithDatabase.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using TodoWithDatabase.Models.DTOs;
using TodoWithDatabase.Services.Interfaces;
using AutoMapper;
using TodoWithDatabase.Models.DAOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using TodoWithDatabase.Models.DTO;
using System.Linq;

namespace TodoWithDatabase.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : ControllerBase
    {
        private readonly UserManager<Assignee> _userManager;
        private readonly IAssigneeService _assigneeService;

        public UserController(UserManager<Assignee> userManager, IAssigneeService assigneeService)
        {
            _userManager = userManager;
            _assigneeService = assigneeService;
        }

        [HttpPost("/api/users")]
        [Authorize(Roles = "TodoAdmin")]
        public IActionResult AddAssignee([FromBody]AssigneeToCreateDTO userDTO)
        {
            Assignee assignee = _userManager.FindByNameAsync(userDTO.Name).Result;

            if (assignee == null)
            {
                var newAssignee = _assigneeService.CreateAndReturnNew(userDTO);
                return Created("api/users/" + newAssignee.Id + "/userWithProjects", new { message = "User with the name " + userDTO.Name + " has been succesfully created!" });
            }

            return BadRequest(new { message = "This name is already in use." });
        }

        [HttpGet("/api/users/all")]
        [Authorize(Roles = "TodoAdmin")]
        public IActionResult GetAllAssignees()
        {
            try
            {
                var result = _assigneeService.GetAndTranslateAll();
                return Ok(result);
            } 
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
                //TODO if this project will get a logger, this error will need to be logged
            }
        }

        [HttpGet("/api/users/{userId}/userWithProjects")]
        [Authorize(Roles = "TodoAdmin")]
        public ActionResult<AssigneeWithProjectsDTO> GetAssigneeProjectFor(string userId)
        {
            return GetAssigneeDTOFor(userId);
        }

        [HttpGet("/api/users/me/userWithProjects")]
        [Authorize]
        public ActionResult<AssigneeWithProjectsDTO> GetAssigneeProjectForSelf()
        {
            var userId = User.FindFirst(c => c.Type.Equals("UserId")).Value;
            return GetAssigneeDTOFor(userId);
        }

        private AssigneeWithProjectsDTO GetAssigneeDTOFor(string userId)
        {
            return _assigneeService.GetAndTranslateToAssigneWithProjectsDTO(userId);
        }
    }
}
