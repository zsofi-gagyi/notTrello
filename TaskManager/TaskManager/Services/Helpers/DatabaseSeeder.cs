using System.Collections.Generic;
using TodoWithDatabase.Repository;
using TodoWithDatabase.Models;
using TodoWithDatabase.Models.DAOs;
using TodoWithDatabase.Services.Interfaces;
using TodoWithDatabase.Models.DTOs;
using TodoWithDatabase.Models.DAOs.JoinTables;

namespace TodoWithDatabase.IntegrationTests.Helpers
{
    public static class DatabaseSeeder
    {
        public static void InitializeDatabaseForTestsUsing(this MyContext context, IAssigneeService assigneeService)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var assignee1 = assigneeService.CreateAndReturnNew(new AssigneeToCreateDTO { Name = "user1Name", Password = "user1Password" });
            var assignee2 = assigneeService.CreateAndReturnNew(new AssigneeToCreateDTO { Name = "user2Name", Password = "user2Password" });

            var projectToEdit = new Project { Title = "projectToEdit" };
            var sharedProject = new Project { Title = "sharedProject" };
            context.Projects.AddRange(projectToEdit, sharedProject);
            context.SaveChanges();

            var assignee1projectToEdit = new AssigneeProject(assignee1, projectToEdit);
            var assignee1sharedProject = new AssigneeProject(assignee1, sharedProject);
            var assignee2sharedProject = new AssigneeProject(assignee2, sharedProject);
            context.AssigneeProjects.AddRange(assignee1projectToEdit, assignee1sharedProject, assignee2sharedProject);
            context.SaveChanges();
        }

        public static void CreateProjectToEditFor(this MyContext context, Assignee assignee1)
        {
            var projectToDelete = new Project { Title = "projectToEdit" };
            context.Projects.AddRange(projectToDelete);
            context.SaveChanges();

            var assignee1projectToDelete = new AssigneeProject(assignee1, projectToDelete);
            context.AssigneeProjects.AddRange(assignee1projectToDelete);
            context.SaveChanges();
        }

        public static List<Card> GetSeedingTodos(List<Assignee> assignees)
        {
            List<Card> result = new List<Card>();
            /* Todo t1 = new Todo("task1", assignees[0]);
             t1.Id = 3;
             Todo t2 = new Todo("task2", assignees[1]);
             t2.Id = 4; */

            return result;
        }
    }
}