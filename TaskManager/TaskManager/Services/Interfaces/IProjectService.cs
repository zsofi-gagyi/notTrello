using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoWithDatabase.Models;
using TodoWithDatabase.Models.DAOs;

namespace TodoWithDatabase.Services.Interfaces
{
    public interface IProjectService
    {
       List<Project> GetAllFor(string userName);

       Project GetWithCards(string projectId);

        Project Get(string projectId);

        bool userCollaboratesOnProject(string assigneeName, string projectId);

        void Save(Project project);

        void Update(Project project);
    }
}
