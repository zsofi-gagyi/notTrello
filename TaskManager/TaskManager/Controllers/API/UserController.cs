using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using TodoWithDatabase.Models.DTOs;
using TodoWithDatabase.Services.Interfaces;
using TodoWithDatabase.Models.DAOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace TodoWithDatabase.Controllers.API
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
            var assignee = _userManager.FindByNameAsync(userDTO.Name).Result;

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
    }
}
