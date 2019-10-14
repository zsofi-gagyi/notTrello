﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using TaskManager.Services.Interfaces;
using System.Threading.Tasks;
using TaskManager.Models.DTOs;

namespace TaskManager.Controllers.API
{
    [ApiController]
    [Route("/api/users")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserWithCardsController : ControllerBase
    {
        private readonly IAssigneeService _assigneeService;

        public UserWithCardsController(IAssigneeService assigneeService)
        {
            _assigneeService = assigneeService;
        }

        [HttpGet("{userId}/userWithCards")]
        [Authorize(Roles = "TodoAdmin")]
        public Task<AssigneeWithCardsDTO> GetAssigneeCardFor(string userId)
        {
            return _assigneeService.GetAndTranslateToAssigneWithCardsDTOAsync(userId);
        }

        [HttpGet("me/userWithCards")]
        [Authorize]
        public Task<AssigneeWithCardsDTO> GetAssigneeCardForSelf()
        {
            var userId = User.FindFirst(claim => claim.Type.Equals("Id")).Value;
            return _assigneeService.GetAndTranslateToAssigneWithCardsDTOAsync(userId);
        }
    }
}