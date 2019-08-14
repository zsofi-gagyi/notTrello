using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using TodoWithDatabase.Services.Interfaces;
using TodoWithDatabase.Models.DTO;

namespace TodoWithDatabase.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserWithProjectsController : ControllerBase
    {
        private readonly IAssigneeService _assigneeService;

        public UserWithProjectsController(IAssigneeService assigneeService)
        {
            _assigneeService = assigneeService;
        }

        [HttpGet("/api/users/{userId}/userWithProjects")]
        [Authorize(Roles = "TodoAdmin")]
        public ActionResult<AssigneeWithProjectsDTO> GetAssigneeProjectFor(string userId)
        {
            return GetAssigneeWithProjectsDTOFor(userId);
        }

        [HttpGet("/api/users/me/userWithProjects")]
        [Authorize]
        public ActionResult<AssigneeWithProjectsDTO> GetAssigneeProjectForSelf()
        {
            var userId = User.FindFirst(c => c.Type.Equals("Id")).Value;
            return GetAssigneeWithProjectsDTOFor(userId);
        }

        private AssigneeWithProjectsDTO GetAssigneeWithProjectsDTOFor(string userId)
        {
            return _assigneeService.GetAndTranslateToAssigneWithProjectsDTO(userId);
        }
    }
}