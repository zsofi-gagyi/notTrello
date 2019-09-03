using System.Collections.Generic;
using TaskManager.Models.DAOs;
using TaskManager.Models.DTOs;

namespace TaskManager.Services.Interfaces
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

        Project TranslateToProject(ProjectWithCardsDTO projectDTO);

        void Delete(string projectId);
    }
}
