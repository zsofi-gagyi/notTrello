using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using TaskManager.Models.DTOs;
using TaskManager.Services.Interfaces;
using TaskManager.Models.DAOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace TaskManager.Controllers.API
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
        public async Task<IActionResult> AddAssignee([FromBody]AssigneeToCreateDTO userDTO)
        {
            var assignee = await _userManager.FindByNameAsync(userDTO.Name);

            if (assignee == null)
            {
                var newAssignee = await _assigneeService.CreateAndReturnNewAsync(userDTO);
                var responseMessage = new { message = "User with the name " + userDTO.Name + " has been succesfully created!" };
                return Created("api/users/" + newAssignee.Id + "/userWithProjects", responseMessage );
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
