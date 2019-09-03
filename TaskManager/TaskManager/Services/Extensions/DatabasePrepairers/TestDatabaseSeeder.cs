using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore;
using TaskManager.Repository;
using TaskManager.Models.DAOs;
using TaskManager.Services.Interfaces;
using TaskManager.Models.DTOs;
using System.Threading.Tasks;

namespace TaskManager.Services.Extensions.DatabaseSeeders
{
    public static class TestDatabaseSeeder 
    {
        public static async Task InitializeDatabaseForAPITestsUsing(this MyContext context, IAssigneeService assigneeService)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var assignee1 = await assigneeService.CreateAndReturnNewAsync(new AssigneeToCreateDTO { Name = "user1Name", Password = "user1Password" });
            var assignee2 = await assigneeService.CreateAndReturnNewAsync(new AssigneeToCreateDTO { Name = "user2Name", Password = "user2Password" });

            var projectToEdit = new Project { Title = "projectToEdit" };
            var sharedProject = new Project { Title = "sharedProject" };
            context.Projects.AddRange(projectToEdit, sharedProject);
            context.SaveChanges();

            context.SaveAssigneeProjectsFor(new List<Assignee> { assignee1 }, projectToEdit);
            context.SaveAssigneeProjectsFor(new List<Assignee> { assignee1, assignee2 }, sharedProject);

            context.SaveChanges();
        }

        public static async Task InitializeDatabaseForHtmlTestsUsing(this MyContext context, IAssigneeService assigneeService, IProjectService projectService)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var user1 = await context.CreateAndDetachGuestAssignee("user1", assigneeService);
            var user2 = await context.CreateAndDetachGuestAssignee("user2", assigneeService);

            var project = new Project
            {
                Title = "project title",
                Description = "project description",
            };
            projectService.Save(project);
            context.Entry(project).State = EntityState.Detached;

            context.SaveAssigneeProjectsFor(new List<Assignee> { user1, user2 }, project);

            var doneCard = new Card
            {
                Title = "done card title",
                Description = "done card description",
                Deadline = DateTime.Now - new TimeSpan(2, 3, 12, 0),
                Done = true,
                Project = project
            };
            context.SaveAssigneeCardsFor(new List<Assignee> { user1, user2 }, doneCard);

            var toDoCard = new Card
            {
                Title = "todo card title",
                Description = "todo card description",
                Deadline = DateTime.Now + new TimeSpan(100, 7, 45, 0),
                Done = false,
                Project = project
            };
            context.SaveAssigneeCardsFor(new List<Assignee> { user1 }, toDoCard);

            project.Cards = new List<Card> { doneCard, toDoCard };

            context.SetToModified(new List<object> { user1, user2, project });
            context.SaveChanges();
        }

        public static void CreateTestProjectToEditFor(this MyContext context, Assignee assignee)
        {
            var projectToEdit = new Project { Title = "projectToEdit" };
            context.Projects.Add(projectToEdit);
            context.SaveChanges();

            context.SaveAssigneeProjectsFor(new List<Assignee> { assignee }, projectToEdit);
            context.SaveChanges();
        }
    }
}
