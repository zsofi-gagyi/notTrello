using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using TodoWithDatabase.Models;
using TodoWithDatabase.Repository;
using TodoWithDatabase.Services.Extensions;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace TodoWithDatabase.Services
{
    public class AssigneeService :  IAssigneeService
    {
        readonly MyContext _myContext;
        readonly IMapper _mapper;
        readonly UserManager<Assignee> _userManager;
        readonly SignInManager<Assignee> _signInManager;

        public AssigneeService(MyContext myContext, IMapper mapper, UserManager<Assignee> userManager, SignInManager<Assignee> signInManager)
        {
            _myContext = myContext;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public List<Assignee> GetAll()
        {
            return _myContext.Assignees.Include(a => a.Todos).ToList();
        }

        public List<AssigneeDTO> GetAndTranslateAll()
        {
            List<Assignee> assignees = GetAll().ToList();
            List<AssigneeDTO> result = new List<AssigneeDTO>();
            assignees.ForEach(a => result.Add(a.ToDtoUsingMapper(_mapper)));
            return result;
        }

        public AssigneeDTO GetAndTranslate(string id)
        {
            Assignee assignee = Get(id);
            AssigneeDTO result = assignee.ToDtoUsingMapper(_mapper);
            return result;
        }

        public Assignee Get(string assigneeId)
        {
            return _myContext.Assignees.Where(a => a.Id.Equals(assigneeId)).SingleOrDefault();
        }

        public bool Exists(string assigneeId)
        {
            return _myContext.Assignees.Where(a => a.Id.Equals(assigneeId)).Count() != 0;
        }

        public Assignee FindByName(string name)
        {
            return _myContext.Assignees.Where(a => a.UserName.Equals(name)).Include(a => a.Todos).SingleOrDefault();
        }

        public void SaveNew(string name, string password)
        {
            var newAssignee = new Assignee { UserName = name };
            _userManager.CreateAsync(newAssignee, password).Wait();
            _userManager.UpdateSecurityStampAsync(newAssignee).Wait();
            _userManager.AddToRoleAsync(newAssignee, "TodoUser").Wait();
            _signInManager.SignInAsync(newAssignee, false).Wait();
        }

        public Assignee SaveAndReturnNew(string name, string password)
        {
            var newAssignee = new Assignee { UserName = name };
            _userManager.CreateAsync(newAssignee, password).Wait();
            _userManager.UpdateSecurityStampAsync(newAssignee).Wait();
            _userManager.AddToRoleAsync(newAssignee, "TodoUser").Wait();

            return newAssignee;
        }
    }
}
