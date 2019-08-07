using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoWithDatabase.Models.DAOs;

namespace TodoWithDatabase.Services.Interfaces
{
    public interface IAssigneeService
    {
        Assignee FindByName(string name);

        /*
        List<AssigneeDTO> GetAndTranslateAll();

        AssigneeDTO GetAndTranslate(string id);
        */
    }
}
