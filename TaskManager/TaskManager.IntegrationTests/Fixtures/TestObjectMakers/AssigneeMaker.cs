using System;
using System.Collections.Generic;
using TaskManager.Models.DAOs;
using TaskManager.Models.DAOs.JoinTables;

namespace TaskManager.IntegrationTests.Fixtures.TestObjectMakers
{
    public static class AssigneeMaker
    {
        public static Assignee MakeAssigneeWithProject() 
        {
            var assignee1 = new Assignee { Id = "1", UserName = "user1Name" };
            var assignee2 = new Assignee { Id = "2", UserName = "user2Name" };

            var card3 = new Card
            {
                Id = new Guid("00000000-0000-0000-0000-000000000003"),
                Title = "card3Title",
                Description = "card3Description",
                Deadline = DateTime.Parse("2019-12-08T12:00:00"),
                Done = false
            };
            card3.AssigneeCards = new List<AssigneeCard>
            {
                new AssigneeCard(assignee1, card3)
            };

            var project4 = new Project
            {
                Id = new Guid("00000000-0000-0000-0000-000000000004"),
                Title = "project4Title",
                Description = "project4Description",
                Cards = new List<Card> { card3 }
            };

            project4.AssigneeProjects = new List<AssigneeProject>
            {
                new AssigneeProject(assignee1, project4),
                new AssigneeProject(assignee2, project4)
            };

            assignee1.AssigneeProjects.Add(new AssigneeProject(assignee1, project4));

            return assignee1;
        }

        public static Assignee MakeUser()
        {
            return new Assignee { Id = "userId", UserName = "user" };
        }

        public static Assignee MakeCollaborator()
        {
            return new Assignee { Id = "collaboratorId", UserName = "collaborator" };
        }
    }
}
