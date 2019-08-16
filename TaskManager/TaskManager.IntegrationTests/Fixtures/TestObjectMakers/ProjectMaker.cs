using System;
using System.Collections.Generic;
using TodoWithDatabase.Models.DAOs;
using TodoWithDatabase.Models.DAOs.JoinTables;

namespace TaskManager.IntegrationTests.Fixtures.TestObjectMakers
{
    public static class ProjectMaker
    {
        public static Project Make()
        {
            var user1 = new Assignee { UserName = "user1", Id = "1t" };
            var user2 = new Assignee { UserName = "user2", Id = "2" };

            var project = new Project
            {
                Id = new Guid(),
                Title = "project title",
                Description = "project description",
            };

            var user1Project = new AssigneeProject(user1, project);
            var user2Project = new AssigneeProject(user2, project);
            project.AssigneeProjects = new List<AssigneeProject> { user1Project, user2Project };
            user1.AssigneeProjects = new List<AssigneeProject> { user1Project };

            var doneCard = new Card
            {
                Id = new Guid(),
                Title = "done card title",
                Description = "done card description",
                Deadline = DateTime.Now - new TimeSpan(2, 3, 12, 0),
                Done = true,
                Project = project
            };

            var user1DoneCard = new AssigneeCard(user1, doneCard);
            var user2DoneCard = new AssigneeCard(user2, doneCard);
            doneCard.AssigneeCards = new List<AssigneeCard> { user1DoneCard, user2DoneCard };

            var toDoCard = new Card
            {
                Id = new Guid(),
                Title = "todo card title",
                Description = "todo card description",
                Deadline = DateTime.Now + new TimeSpan(100, 7, 45, 0),
                Done = false,
                Project = project
            };

            var user1ToDoCard = new AssigneeCard(user1, toDoCard);
            toDoCard.AssigneeCards.Add(user1ToDoCard);

            project.Cards = new List<Card> { doneCard, toDoCard };

            return project;
        }
    }
}