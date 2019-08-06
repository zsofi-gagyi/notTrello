using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoWithDatabase.Models;

namespace TodoWithDatabase.Services
{
    public interface IAssigneeService
    {
        List<Assignee> GetAll();

        List<AssigneeDTO> GetAndTranslateAll();

        AssigneeDTO GetAndTranslate(string id);

        Assignee Get(string id);

        bool Exists(string id);
        Assignee FindByName(string name);

        void SaveNew(string name, string password);

        Assignee SaveAndReturnNew(string name, string password);
    }
}
