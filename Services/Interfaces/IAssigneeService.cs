using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoWithDatabase.Models;

namespace TodoWithDatabase.Services.Interfaces
{
    public interface IAssigneeService
    {
        Assignee FindByName(string name);

        void SaveNew(string name, string password);
        /*
        List<Assignee> GetAll();

        List<AssigneeDTO> GetAndTranslateAll();

        AssigneeDTO GetAndTranslate(string id);

        Assignee Get(string id);

        bool Exists(string id);
       

        

        Assignee SaveAndReturnNew(string name, string password);
        */
    }
}
