using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoWithDatabase.Models;
using TodoWithDatabase.Models.DAOs;
using TodoWithDatabase.Models.DTOs;

namespace TodoWithDatabase.Services.Interfaces
{
    public interface IAssigneeService
    {
        Assignee GetWithAssigneeCards(string name);

        Assignee CreateAndReturnNew(AssigneeToCreateDTO assigneeDTO);

        void CreateAndSignIn(string name, string password);

        /*
        List<AssigneeDTO> GetAndTranslateAll();

        AssigneeDTO GetAndTranslate(string id);
        */
    }
}
