using System.Collections.Generic;
using System.Linq;
using TaskManager.Models.DAOs;
using TaskManager.Models.DTOs;

namespace TaskManager.TestUtilities.TestObjectMakers
{
    public static class AssigneeWithProjectsDTOMaker
    {
        public static AssigneeWithProjectsDTO MakeFrom(Assignee assignee)
        {
            var assigneeWithProjects = new AssigneeWithProjectsDTO
            {
                Id = assignee.Id,
                UserName = assignee.UserName                    
            };

            var project4 = new ProjectWithCardsDTO
            {
                Id = assignee.AssigneeProjects.First().Project.Id.ToString(),
                Title = assignee.AssigneeProjects.First().Project.Title,
                Description = assignee.AssigneeProjects.First().Project.Description,
            };

            assigneeWithProjects.ProjectWithCardsDTOs = new List<ProjectWithCardsDTO> { project4 };

            var assignee1 = new AssigneeDTO { Id = assignee.Id, UserName = assignee.UserName };

            var assignee2Id = assignee.AssigneeProjects.First().Project.AssigneeProjects.Where(ap => ap.AssigneeId != assignee.Id).First().AssigneeId;
            var assignee2Name = assignee.AssigneeProjects.First().Project.AssigneeProjects.Where(ap => ap.AssigneeId != assignee.Id).First().Assignee.UserName;
            var assignee2 = new AssigneeDTO { Id = assignee2Id, UserName = assignee2Name };

            project4.AssigneeDTOs = new List<AssigneeDTO> { assignee1, assignee2 };

            var card3 = new CardWithAssigneesDTO
            {
                Id = assignee.AssigneeProjects.First().Project.Cards.First().Id.ToString(),
                Title = assignee.AssigneeProjects.First().Project.Cards.First().Title,
                Description = assignee.AssigneeProjects.First().Project.Cards.First().Description,
                Deadline = assignee.AssigneeProjects.First().Project.Cards.First().Deadline,
                Done = assignee.AssigneeProjects.First().Project.Cards.First().Done,
            };

            card3.AssigneeDTOs = new List<AssigneeDTO> { assignee1 };

            project4.CardWithAssigneesDTOs = new List<CardWithAssigneesDTO> { card3 };

            return assigneeWithProjects;
        }
    }
}
