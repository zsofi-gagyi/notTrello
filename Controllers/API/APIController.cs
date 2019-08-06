using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TodoWithDatabase.Services;
using TodoWithDatabase.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using TodoWithDatabase.Models.DTOs;

namespace TodoWithDatabase.Controllers
{
    [ApiController]
    public class APIController : ControllerBase
    {
        private readonly IAssigneeService _assigneeService;

        public APIController(IAssigneeService assigneeService)
        {
            _assigneeService = assigneeService;
        }
        /*
        [HttpPost("/api/add-assignee")]
        [Authorize(Roles = "TodoAdmin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult AddAssignee([FromBody]AssigneeToCreateDTO assigneeDTO) 
        {
            Assignee assignee = _assigneeService.FindByName(assigneeDTO.Name);

            if (assignee == null)
            {
                var newAssignee = _assigneeService.SaveAndReturnNew(assigneeDTO.Name, assigneeDTO.Password);
                return Created( "api/assignees/" + newAssignee.Id,  new { message = "assignee named " + assigneeDTO.Name + " succesfully created!" });
            }

            return BadRequest(new { message = "assignee already exists!" });
        }

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
