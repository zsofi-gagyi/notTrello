﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using TaskManager.Services.Interfaces;
using TaskManager.Models.DTOs;
using System.Threading.Tasks;

namespace TaskManager.Controllers.API
{
    [ApiController]
    [Route("/api/users")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserWithProjectsController : ControllerBase
    {
        private readonly IAssigneeService _assigneeService;

        public UserWithProjectsController(IAssigneeService assigneeService)
        {
            _assigneeService = assigneeService;
        }

        [HttpGet("{userId}/userWithProjects")]
        [Authorize(Roles = "TodoAdmin")]
        public Task<AssigneeWithProjectsDTO> GetAssigneeProjectFor(string userId)
        {
            return GetAssigneeWithProjectsDTOForAsync(userId);
        }

        [HttpGet("me/userWithProjects")]
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