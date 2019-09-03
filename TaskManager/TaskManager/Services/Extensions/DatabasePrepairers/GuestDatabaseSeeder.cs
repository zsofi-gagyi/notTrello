using System.Collections.Generic;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TaskManager.Repository;
using TaskManager.Models.DAOs;
using TaskManager.Services.Interfaces;
using TaskManager.Models.DTOs;
using TaskManager.Models.DAOs.JoinTables;
using System.Threading.Tasks;

namespace TaskManager.Services.Extensions.DatabaseSeeders
{
    public static class GuestDatabaseSeeder
    {
        public static void EnsureDatabaseHasStandardGuestData(this IServiceCollection services)
        {
            var context = services.BuildServiceProvider().GetRequiredService<MyContext>();
            var assigneeService = services.BuildServiceProvider().GetRequiredService<IAssigneeService>();
            var projectService = services.BuildServiceProvider().GetRequiredService<IProjectService>();

            if (context.Assignees.Where(a => a.UserName.Equals("Guest")).Count() > 0)
            {
                DeleteGuestData(context, projectService);
            } 

            context.CreateGuestData(assigneeService, projectService).Wait();
        }

        private static void DeleteGuestData(MyContext context, IProjectService projectService)
        {
            var guestToDelete = context.Assignees.First(a => a.UserName.Equals("Guest"));
            var aliceToDelete = context.Assignees.First(a => a.UserName.Equals("Alice"));
            var bobToDelete = context.Assignees.First(a => a.UserName.Equals("Bob"));

            var projectsToRemove = context.AssigneeProjects
                 .Where(ap => ap.Assignee
                                    .Id
                                    .Equals(guestToDelete.Id))
                 .Select(ap => ap.Project)
                 .ToList();

            foreach (var project in projectsToRemove)
            {
                projectService.Delete(project.Id.ToString());
            }

            context.Assignees.RemoveRange(guestToDelete, aliceToDelete, bobToDelete);
            context.SaveChanges();
        }

        private static async Task CreateGuestData(this MyContext context, IAssigneeService assigneeService, IProjectService projectService)
        {
            var guest = await context.CreateAndDetachGuestAssignee("Guest", assigneeService);
            var alice = await context.CreateAndDetachGuestAssignee("Alice", assigneeService);
            var bob = await context.CreateAndDetachGuestAssignee("Bob", assigneeService);

            var soloProject = context.CreateAndDetachGuestProject("solo", projectService);
            var sharedProject = context.CreateAndDetachGuestProject("shared", projectService);

            context.SaveAssigneeProjectsFor(new List<Assignee> { guest }, soloProject);
            context.SaveAssigneeProjectsFor(new List<Assignee> { guest, alice, bob }, sharedProject);

            soloProject.Cards = CreateCardsForSoloProject(soloProject);
            sharedProject.Cards = CreateCardsForSharedProject(context, sharedProject, guest, alice, bob);

            context.SetToModified(new List<object> { soloProject, sharedProject, guest, alice, bob });

            context.SaveChanges();
        }

        public static async Task<Assignee> CreateAndDetachGuestAssignee(this MyContext context, string name, IAssigneeService assigneeService)
        {
            var assignee = await assigneeService.CreateAndReturnNewAsync(new AssigneeToCreateDTO { Name = name, Password = name + "1234." });
            context.Entry(assignee).State = EntityState.Detached;

            return assignee;
        }

        private static Project CreateAndDetachGuestProject(this MyContext context, string projectType, IProjectService projectService)
        {
            var project = new Project
            {
                Title = projectType.Equals("solo") ? "Personal project" : "Team project",
                Description = projectType.Equals("solo") ? 
                "As an individual, I'm planning to... lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor " +
                "incididunt ut labore et dolore magna aliqua. Dolor sed viverra ipsum nunc aliquet bibendum enim. In massa tempor nec feugiat. " +
                "Nunc aliquet bibendum enim facilisis gravida. Nisl nunc mi ipsum faucibus vitae aliquet nec ullamcorper. Amet luctus venenatis " +
                "lectus magna fringilla. Volutpat maecenas volutpat blandit aliquam etiam erat velit scelerisque in. Egestas egestas fringilla " +
                "phasellus faucibus scelerisque eleifend. Sagittis orci a scelerisque purus semper eget duis. Nulla pharetra diam sit amet " +
                "nisl suscipit."
                :
                "As a team, we're planning to... lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod " +
                "tempor incididunt ut labore et dolore magna aliqua. Dolor sed viverra ipsum nunc aliquet bibendum enim. In massa " +
                "tempor nec feugiat. Nunc aliquet bibendum enim facilisis gravida. Nisl nunc mi ipsum faucibus vitae aliquet nec " +
                "ullamcorper. Amet luctus venenatis lectus magna fringilla. Volutpat maecenas volutpat blandit aliquam etiam erat " +
                "velit scelerisque in."
            };

            projectService.Save(project);
            context.Entry(project).State = EntityState.Detached;

            return project;
        }

        private static List<Card> CreateCardsForSoloProject(Project soloProject)
        {
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

            return new List<Card> { soloProjectDoneCard, soloProjectToDoCard, soloProjectPastDeadlineCard };
        }

        private static List<Card> CreateCardsForSharedProject(MyContext context, Project sharedProject, Assignee guest, Assignee alice, Assignee bob)
        {
            var sharedProjectDoneCard = CreateSharedProjectDoneCardFor(sharedProject, alice, context);
            var sharedProjectToDoCard = CreateSharedProjectToDoCardFor(sharedProject, guest, alice, bob, context);
            var sharedProjectPastDeadlineCard = CreateSharedProjectPastDeadlineCardFor(sharedProject, alice, bob, context);
            return new List<Card> { sharedProjectDoneCard, sharedProjectToDoCard, sharedProjectPastDeadlineCard };
        }

        private static Card CreateSharedProjectDoneCardFor(Project sharedProject, Assignee alice, MyContext context)
        {
            var card = new Card
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
            context.SaveAssigneeCardsFor(new List<Assignee> { alice }, card);

            return card;
        }

        private static Card CreateSharedProjectToDoCardFor(Project sharedProject, Assignee guest, Assignee alice, Assignee bob, MyContext context)
        {
            var card = new Card
            {
                Title = "Blandit aliquam etiam erat",
                Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut " +
                    "labore et dolore magna aliqua. Dolor sed viverra ipsum nunc aliquet bibendum enim. In massa tempor nec feugiat. ",
                Deadline = DateTime.Now + new TimeSpan(100, 7, 45, 0),
                Done = false,
                Project = sharedProject
            };
            context.SaveAssigneeCardsFor(new List<Assignee> { guest, alice, bob }, card);

            return card;
        }

        private static Card CreateSharedProjectPastDeadlineCardFor(Project sharedProject, Assignee alice, Assignee bob, MyContext context)
        {
            var card = new Card
            {
                Title = "Facilisis gravida, nisl nunc mi",
                Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor " +
                    "incididunt ut labore et dolore magna aliqua. Dolor sed viverra ipsum nunc aliquet bibendum enim. In massa tempor nec feugiat. " +
                    "Nunc aliquet bibendum enim facilisis gravida. Nisl nunc mi ipsum faucibus vitae aliquet nec ullamcorper.",
                Deadline = DateTime.Now - new TimeSpan(2, 1, 30, 0),
                Done = false,
                Project = sharedProject
            };
            context.SaveAssigneeCardsFor(new List<Assignee> { alice, bob }, card);

            return card;
        }

        public static void SetToModified(this MyContext context, List<Object> objects)
        {
            foreach(var obj in objects)
            {
                context.Entry(obj).State = EntityState.Modified;
            }
        }

        public static void SaveAssigneeProjectsFor(this MyContext context, List<Assignee> assignees, Project project)
        {
            foreach(var assignee in assignees)
            {
                context.AssigneeProjects.Add(new AssigneeProject(assignee, project));
            }
        }

        public static void SaveAssigneeCardsFor(this MyContext context, List<Assignee> assignees, Card card)
        {
            foreach (var assignee in assignees)
            {
                context.AssigneeCards.Add(new AssigneeCard(assignee, card));
            }
        }
    }
}