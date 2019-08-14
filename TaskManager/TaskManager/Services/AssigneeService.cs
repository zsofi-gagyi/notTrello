using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using TodoWithDatabase.Models.DAOs;
using TodoWithDatabase.Repository;
using TodoWithDatabase.Services.Extensions;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using TodoWithDatabase.Services.Interfaces;
using TodoWithDatabase.Models.DTOs;
using TodoWithDatabase.Models;
using TodoWithDatabase.Models.DTO;

namespace TodoWithDatabase.Services
{
    public class AssigneeService :  IAssigneeService
    {
        readonly UserManager<Assignee> _userManager;
        readonly SignInManager<Assignee> _signInManager;
        readonly RoleManager<IdentityRole> _roleManager;
        readonly MyContext _myContext;
        readonly IMapper _mapper;

        public AssigneeService(UserManager<Assignee> userManager, SignInManager<Assignee> signInManager, RoleManager<IdentityRole> roleManager, MyContext myContext, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _myContext = myContext;
            _mapper = mapper;
        }

        public Assignee GetWithAssigneeCards(string name)
        {
            return _myContext.Assignees
                .Where(a => a.UserName.Equals(name))
                .Include(a => a.AssigneeCards)
                .SingleOrDefault();
        }

        public Assignee CreateAndReturnNew(AssigneeToCreateDTO assigneeDTO)
        {
            var newAssignee = new Assignee { UserName = assigneeDTO.Name };
            _userManager.CreateAsync(newAssignee, assigneeDTO.Password).Wait();
            _userManager.AddToRoleAsync(newAssignee, "TodoUser").Wait();
            return newAssignee;
        }

        public void CreateAndSignIn(string name, string password)
        {
            var newAssignee = new Assignee { UserName = name };
            _userManager.CreateAsync(newAssignee, password).Wait();
            _userManager.AddToRoleAsync(newAssignee, "TodoUser").Wait();
            _signInManager.SignInAsync(newAssignee, false).Wait();
        }

        public List<AssigneeDTO> GetAndTranslateAll()
        {
            List<Assignee> assignees = _myContext.Assignees.ToList();
            List<AssigneeDTO> result = new List<AssigneeDTO>();
            assignees.ForEach(a => result.Add(_mapper.Map<AssigneeDTO>(a)));
            return result;
        }

        public AssigneeWithProjectsDTO GetAndTranslateToAssigneWithProjectsDTO(string userId)
        {
            var assignee = _myContext.Assignees
                .Where(a => a.Id.Equals(userId))

                .Include(c => c.AssigneeProjects)
                    .ThenInclude(ap => ap.Project)
                        .ThenInclude(p => p.AssigneeProjects)
                            .ThenInclude(ap => ap.Assignee)

                .Include(c => c.AssigneeProjects)
                    .ThenInclude(ap => ap.Project)
                        .ThenInclude(p => p.Cards)
                            .ThenInclude(c => c.AssigneeCards)
                                .ThenInclude(ac => ac.Assignee)

                .FirstOrDefault();

            var assigneeWithProjectsDTO = _mapper.Map<AssigneeWithProjectsDTO>(assignee);

            string role = _userManager.GetRolesAsync(assignee).Result.FirstOrDefault().ToString();
            assigneeWithProjectsDTO.Role = role;

            return assigneeWithProjectsDTO;
        }
    }
}
