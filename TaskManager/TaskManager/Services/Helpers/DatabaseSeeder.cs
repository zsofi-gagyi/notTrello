using System.Collections.Generic;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TodoWithDatabase.Repository;
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

        public static void EnsureDatabaseHasGuestData(this IServiceCollection services)
        {
            var context = services.BuildServiceProvider().GetRequiredService<MyContext>();

            if (context.Assignees.Where(a => a.UserName.Equals("Guest")).Count() != 1) 
            {
                var assigneeService = services.BuildServiceProvider().GetRequiredService<IAssigneeService>();
                var projectService = services.BuildServiceProvider().GetRequiredService<IProjectService>();

                var guest = assigneeService.CreateAndReturnNew(new AssigneeToCreateDTO { Name = "Guest", Password = "guest" });
                var alice = assigneeService.CreateAndReturnNew(new AssigneeToCreateDTO { Name = "Alice", Password = "a" });
                var bob = assigneeService.CreateAndReturnNew(new AssigneeToCreateDTO { Name = "Bob", Password = "b" });
               
                context.CreateProjects(guest, alice, bob, projectService);
            }
        }

        private static void CreateProjects(this MyContext context, Assignee guest, Assignee alice, Assignee bob, IProjectService projectService)
        {
            context.Entry(guest).State = EntityState.Detached;
            context.Entry(alice).State = EntityState.Detached;
            context.Entry(bob).State = EntityState.Detached;

            var soloProject = new Project
            {
                Title = "Personal project",
                Description = "As an individual, I'm planning to... lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor " +
                    "incididunt ut labore et dolore magna aliqua. Dolor sed viverra ipsum nunc aliquet bibendum enim. In massa tempor nec feugiat. " +
                    "Nunc aliquet bibendum enim facilisis gravida. Nisl nunc mi ipsum faucibus vitae aliquet nec ullamcorper. Amet luctus venenatis " +
                    "lectus magna fringilla. Volutpat maecenas volutpat blandit aliquam etiam erat velit scelerisque in. Egestas egestas fringilla " +
                    "phasellus faucibus scelerisque eleifend. Sagittis orci a scelerisque purus semper eget duis. Nulla pharetra diam sit amet " +
                    "nisl suscipit.",
            };

            projectService.Save(soloProject);
            context.Entry(soloProject).State = EntityState.Detached;

            var sharedProject = new Project
            {
                Title = "Team project",
                Description = "As a team, we're planning to... lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod " +
                    "tempor incididunt ut labore et dolore magna aliqua. Dolor sed viverra ipsum nunc aliquet bibendum enim. In massa " +
                    "tempor nec feugiat. Nunc aliquet bibendum enim facilisis gravida. Nisl nunc mi ipsum faucibus vitae aliquet nec " +
                    "ullamcorper. Amet luctus venenatis lectus magna fringilla. Volutpat maecenas volutpat blandit aliquam etiam erat " +
                    "velit scelerisque in.",
            };

            projectService.Save(sharedProject);
            context.Entry(sharedProject).State = EntityState.Detached;
            
            var guestSoloProject = new AssigneeProject(guest, soloProject);
            var guestSharedProject = new AssigneeProject(guest, sharedProject);
            context.AssigneeProjects.AddRange(guestSoloProject, guestSharedProject);

            var aliceSharedProject = new AssigneeProject(alice, sharedProject);
            var bobSharedProject = new AssigneeProject(bob, sharedProject);
            context.AssigneeProjects.AddRange(guestSharedProject, aliceSharedProject, bobSharedProject);

            var soloProjectDoneCard = new Card
            {
                Title = "Lorem ipsum dolor sit amet, consectetur adipiscing elit",
                Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor " +
                    "incididunt ut labore et dolore magna aliqua. Dolor sed viverra ipsum nunc aliquet bibendum enim. In massa tempor nec feugiat. " +
                    "Nunc aliquet bibendum enim facilisis gravida. Nisl nunc mi ipsum faucibus vitae aliquet nec ullamcorper.",
                Deadline = DateTime.Now - new TimeSpan(2, 3, 12, 0),
                Done = true,
                Project = soloProject
            };

            var soloProjectToDoCard = new Card
            {
                Title = "Sed do eiusmod tempor incididunt ut labore",
                Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor " +
                    "incididunt ut labore et dolore magna aliqua. Dolor sed viverra ipsum nunc aliquet bibendum enim. In massa tempor nec feugiat. " +
                    "Nunc aliquet bibendum enim facilisis gravida.",
                Deadline = DateTime.Now + new TimeSpan(100, 7, 45, 0),
                Done = false,
                Project = soloProject
            };

            var soloProjectPastDeadlineCard = new Card
            {
                Title = "Facilisis gravida, nisl nunc mi",
                Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor " +
                    "incididunt ut labore et dolore magna aliqua.",
                Deadline = DateTime.Now - new TimeSpan(2, 1, 30, 0),
                Done = false,
                Project = soloProject
            };

            soloProject.Cards = new List<Card> { soloProjectDoneCard, soloProjectToDoCard, soloProjectPastDeadlineCard };

            var sharedProjectDoneCard = new Card
            {
                Title = "Enim facilisis gravida",
                Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut " +
                    "labore et dolore magna aliqua. Dolor sed viverra ipsum nunc aliquet bibendum enim. In massa tempor nec feugiat. " +
                    "Nunc aliquet bibendum enim facilisis gravida. Nisl nunc mi ipsum faucibus vitae aliquet nec ullamcorper. Amet luctus " +
                    "venenatis lectus magna fringilla. Volutpat maecenas volutpat blandit aliquam etiam erat velit scelerisque in. Egestas " +
                    "egestas fringilla phasellus faucibus scelerisque eleifend.",
                Deadline = DateTime.Now - new TimeSpan(2, 3, 12, 0),
                Done = true,
                Project = sharedProject
            };

            var aliceSharedProjectDoneCard = new AssigneeCard(alice, sharedProjectDoneCard);
            context.AssigneeCards.Add(aliceSharedProjectDoneCard);

            var sharedProjectToDoCard = new Card
            {
                Title = "Blandit aliquam etiam erat",
                Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut " +
                    "labore et dolore magna aliqua. Dolor sed viverra ipsum nunc aliquet bibendum enim. In massa tempor nec feugiat. ",
                Deadline = DateTime.Now + new TimeSpan(100, 7, 45, 0),
                Done = false,
                Project = sharedProject
            };

            var bobSharedProjectToDoCard = new AssigneeCard(bob, sharedProjectToDoCard);
            var guestSharedProjectToDoCard = new AssigneeCard(guest, sharedProjectToDoCard);
            var aliceSharedProjectToDoCard = new AssigneeCard(alice, sharedProjectToDoCard);
            context.AssigneeCards.AddRange(bobSharedProjectToDoCard, guestSharedProjectToDoCard, aliceSharedProjectToDoCard);

            var sharedProjectPastDeadlineCard = new Card
            {
                Title = "Facilisis gravida, nisl nunc mi",
                Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor " +
                    "incididunt ut labore et dolore magna aliqua. Dolor sed viverra ipsum nunc aliquet bibendum enim. In massa tempor nec feugiat. " +
                    "Nunc aliquet bibendum enim facilisis gravida. Nisl nunc mi ipsum faucibus vitae aliquet nec ullamcorper.",
                Deadline = DateTime.Now - new TimeSpan(2, 1, 30, 0),
                Done = false,
                Project = sharedProject
            };

            var bobSharedProjectPastDeadlineCard = new AssigneeCard(bob, sharedProjectPastDeadlineCard);
            var aliceSharedProjectPastDeadlineCard = new AssigneeCard(alice, sharedProjectPastDeadlineCard);
            context.AssigneeCards.AddRange(bobSharedProjectPastDeadlineCard, aliceSharedProjectPastDeadlineCard);

            sharedProject.Cards = new List<Card> { sharedProjectDoneCard, sharedProjectToDoCard, sharedProjectPastDeadlineCard };

            context.Entry(soloProject).State = EntityState.Modified;
            context.Entry(sharedProject).State = EntityState.Modified;
            context.Entry(guest).State = EntityState.Modified;
            context.Entry(alice).State = EntityState.Modified;
            context.Entry(bob).State = EntityState.Modified;
            context.SaveChanges();
        }
    }
}