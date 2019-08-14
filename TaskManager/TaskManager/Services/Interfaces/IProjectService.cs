using System.Collections.Generic;
using TodoWithDatabase.Models.DAOs;

namespace TodoWithDatabase.Services.Interfaces
{
    public interface IProjectService
    {
       List<Project> GetAllFor(string userName);

       Project GetWithCards(string projectId);

        Project Get(string projectId);

        Project GetWithAssigneeProjects(string projectId);

        bool UserIsCollaboratingOnProject(string assigneeName, string projectId);

        void Save(Project project);

        void Update(Project project);

        void Delete(string projectId);
    }
}
