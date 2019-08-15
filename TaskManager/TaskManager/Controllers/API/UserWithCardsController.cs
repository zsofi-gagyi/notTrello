using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using TodoWithDatabase.Services.Interfaces;
using TodoWithDatabase.Models.DTO;

namespace TodoWithDatabase.Controllers.API
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserWithCardsController : ControllerBase
    {
        private readonly IAssigneeService _assigneeService;

        public UserWithCardsController(IAssigneeService assigneeService)
        {
            _assigneeService = assigneeService;
        }

        [HttpGet("/api/users/{userId}/userWithCards")]
        [Authorize(Roles = "TodoAdmin")]
        public ActionResult<AssigneeWithCardsDTO> GetAssigneeCardFor(string userId)
        {
            return GetAssigneeWithCardsDTOFor(userId);
        }

        [HttpGet("/api/users/me/userWithCards")]
        [Authorize]
        public ActionResult<AssigneeWithCardsDTO> GetAssigneeCardForSelf()
        {
            var userId = User.FindFirst(c => c.Type.Equals("Id")).Value;
            return GetAssigneeWithCardsDTOFor(userId); 
        }

        private AssigneeWithCardsDTO GetAssigneeWithCardsDTOFor(string userId)
        {
            return _assigneeService.GetAndTranslateToAssigneWithCardsDTO(userId);
        }
    }
}