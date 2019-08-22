using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using TodoWithDatabase.Services.Interfaces;
using TodoWithDatabase.Models.DTO;
using System.Threading.Tasks;

namespace TodoWithDatabase.Controllers.API
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
        public Task<AssigneeWithProjectsDTO> GetAssigneeProjectFor(string userId)
        {
            return GetAssigneeWithProjectsDTOForAsync(userId);
        }

        [HttpGet("/api/users/me/userWithProjects")]
        [Authorize]
        public Task<AssigneeWithProjectsDTO> GetAssigneeProjectForSelf()
        {
            var userId = User.FindFirst(c => c.Type.Equals("Id")).Value;
            return GetAssigneeWithProjectsDTOForAsync(userId);
        }

        private Task<AssigneeWithProjectsDTO> GetAssigneeWithProjectsDTOForAsync(string userId)
        {
            return _assigneeService.GetAndTranslateToAssigneWithProjectsDTOAsync(userId);
        }
    }
}