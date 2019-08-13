using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TodoWithDatabase.Services;
using TodoWithDatabase.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using TodoWithDatabase.Models.DTOs;
using TodoWithDatabase.Services.Interfaces;
using AutoMapper;
using TodoWithDatabase.Models.DAOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace TodoWithDatabase.Controllers
{
    [ApiController]
    [Authorize(Roles = "TodoAdmin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly UserManager<Assignee> _userManager;
        private readonly IAssigneeService _assigneeService;

        public UserController(IMapper mapper, UserManager<Assignee> userManager, IAssigneeService assigneeService)
        {
            _mapper = mapper;
            _userManager = userManager;
            _assigneeService = assigneeService;
        }

        [HttpPost("/api/users")]
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
        public IActionResult GetAllAssignees()
        {
            var ok = true;
            var result = new List<AssigneeDTO>();

            try
            {
                result = _assigneeService.GetAndTranslateAll();
            } 
            catch
            {
                ok = false;
            }

            if (ok)
            {
                return Ok(result);
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        /*

        [HttpGet("/api/assignees")]
        [Authorize(Roles = "TodoUser" + "," + "TodoAdmin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult<List<AssigneeDTO>> GetAssignees()
        {
            List<AssigneeDTO> result = _assigneeService.GetAndTranslateAll().ToList();
            return result;
        }

        [HttpGet("/api/assignees/{Id}")]
        [Authorize(Roles = "TodoAdmin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult<AssigneeDTO> GetAssignee([FromRoute(Name = "Id")]string id)
        {
            AssigneeDTO result = _assigneeService.GetAndTranslate(id);
            return result;
        }

        [HttpGet("/api/assignees/me")]
        [Authorize(Roles = "TodoAdmin" + "," + "TodoUser", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult<AssigneeDTO> GetMe()
        {
            var name = User.Identity.Name;
            var id = _assigneeService.FindByName(name).Id;
            AssigneeDTO result = _assigneeService.GetAndTranslate(id);
            return result;
            }
            */
    }
}
