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

namespace TodoWithDatabase.Services
{
    public class AssigneeService :  IAssigneeService
    {
        readonly MyContext _myContext;
        readonly IMapper _mapper;

        public AssigneeService(MyContext myContext, IMapper mapper)
        {
            _myContext = myContext;
            _mapper = mapper;
        }

        public Assignee FindByName(string name)
        {
            return _myContext.Assignees.Where(a => a.UserName.Equals(name)).SingleOrDefault();
        }

        /*
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
        */
    }
}
