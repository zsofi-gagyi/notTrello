using System;
using System.Collections.Generic;
using TaskManager.Models.DAOs;
using TaskManager.Models.DTOs;

namespace TaskManager.Services.Interfaces
{
    public interface IProjectService
    {
       List<Project> GetAllFor(string userName);

       Project GetWithCards(Guid projectId);

        Project Get(Guid projectId);

        Project GetWithAssigneeProjects(Guid projectId);

        bool UserIsCollaboratingOnProject(string assigneeName, Guid projectId);

        void Save(Project project);

        void Update(Project project);

        Project TranslateToProject(ProjectWithCardsDTO projectDTO);

        void Delete(Guid projectId);
    }
}
