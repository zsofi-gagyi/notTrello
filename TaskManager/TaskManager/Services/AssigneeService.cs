using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using TaskManager.Models.DAOs;
using TaskManager.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TaskManager.Services.Interfaces;
using TaskManager.Models.DTOs;
using System.Threading.Tasks;
using System;

namespace TaskManager.Services
{
    public class AssigneeService :  IAssigneeService
    {
        readonly UserManager<Assignee> _userManager;
        readonly SignInManager<Assignee> _signInManager;
        readonly MyContext _myContext;
        readonly IMapper _mapper;

        public AssigneeService(UserManager<Assignee> userManager, SignInManager<Assignee> signInManager, MyContext myContext, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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

        public async Task<Assignee> CreateAndReturnNewAsync(AssigneeToCreateDTO assigneeDTO)
        {
            var newAssignee = new Assignee { UserName = assigneeDTO.Name };

            await _userManager.CreateAsync(newAssignee, assigneeDTO.Password);
            await _userManager.AddToRoleAsync(newAssignee, "TodoUser");

            return newAssignee;
        }

        public async Task CreateAndSignInAsync(string name, string password)
        {
            var newAssignee = new Assignee { UserName = name };

            await _userManager.CreateAsync(newAssignee, password);
            await _userManager.AddToRoleAsync(newAssignee, "TodoUser");
            await _signInManager.SignInAsync(newAssignee, false);
        }

        public async Task CreateAndSignInWithEmailAsync(string name, string email)
        {
            var correctedName = GetInitials(name);
                    // This serves for keeping the names recognizable but anonimized, since anybody 
                    // can get all names by becoming an admin.
                    // Of course, duplicated initials could cause problems, but to solve that, I would 
                    // need to remake the entire way user identities are stored - email adresses should 
                    // be the primary keys, and then names could be changed freely.
                    
            var newAssignee = new Assignee { UserName = correctedName, Email = email };

            await _userManager.CreateAsync(newAssignee, new Guid().ToString() + "Aa1.");
                    // TODO there could be an option for these users (created following 
                    // Google Authentication) to change their passwords, 
                    // so they can access their accounts without using Google

            await _userManager.AddToRoleAsync(newAssignee, "TodoUser");
            await _signInManager.SignInAsync(newAssignee, false);
        }

        private static string GetInitials(string name)
        {
            string[] names = name.Split(" ");
            string result = "Google_user";
            foreach (string namePart in names)
            {
                result = result + "_" + namePart.Substring(0, 1).ToUpper();
            }
            return result;
        }

        public List<AssigneeDTO> GetAndTranslateAll()
        {
            List<Assignee> assignees = _myContext.Assignees.ToList();
            List<AssigneeDTO> result = new List<AssigneeDTO>();
            assignees.ForEach(a => result.Add(_mapper.Map<AssigneeDTO>(a)));

            return result;
        }

        public void Update(Assignee assignee)
        {
            var originalAssignee = _myContext.Find(typeof(Assignee), assignee.Id);
            _myContext.Entry(originalAssignee).State = EntityState.Detached;
            _myContext.Entry(assignee).State = EntityState.Modified;
            _myContext.SaveChanges();
        }

        public async Task<AssigneeWithProjectsDTO> GetAndTranslateToAssigneWithProjectsDTOAsync(string userId)
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

            var rolesEnumerable = await _userManager.GetRolesAsync(assignee);
            var role = rolesEnumerable.FirstOrDefault().ToString();
            assigneeWithProjectsDTO.Role = role;

            return assigneeWithProjectsDTO;
        }

        public async Task<AssigneeWithCardsDTO> GetAndTranslateToAssigneWithCardsDTOAsync(string userId)
        {
            var assignee = _myContext.Assignees
                .Where(a => a.Id.Equals(userId))

                .Include(c => c.AssigneeCards)
                    .ThenInclude(ac => ac.Card)
                        .ThenInclude(c => c.AssigneeCards)
                            .ThenInclude(ac => ac.Assignee)

                .Include(c => c.AssigneeCards)
                    .ThenInclude(ac => ac.Card)
                        .ThenInclude(c => c.Project)
                            .ThenInclude(p => p.AssigneeProjects)
                                .ThenInclude(ap => ap.Assignee)

                .FirstOrDefault();

            var assigneeWithCardsDTO = _mapper.Map<AssigneeWithCardsDTO>(assignee);

            var rolesEnumerable = await _userManager.GetRolesAsync(assignee);
            var role = rolesEnumerable.FirstOrDefault().ToString();
            assigneeWithCardsDTO.Role = role;

            return assigneeWithCardsDTO;
        }
    }
}
