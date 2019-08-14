using System.Collections.Generic;
using TodoWithDatabase.Repository;
using TodoWithDatabase.Models;
using TodoWithDatabase.Models.DAOs;
using TodoWithDatabase.Services.Interfaces;
using TodoWithDatabase.Models.DTOs;

namespace TodoWithDatabase.IntegrationTests.Helpers
{
    public static class DatabaseSeeder
    {
            public static void InitializeDatabaseForTestsUsing(this MyContext myContext, IAssigneeService assigneeService)
            {
                myContext.Database.EnsureDeleted();
                myContext.Database.EnsureCreated();

                var assignee1 = assigneeService.CreateAndReturnNew(new AssigneeToCreateDTO {  Name = "user1Name", Password = "user1Password" });
                
                //List<Assignee> assignees = GetSeedingAssignees();
                //myContext.Assignees.AddRange(assignees);
                // myContext.Cards.AddRange(GetSeedingTodos(assignees));
                // myContext.SaveChanges();
            }

            public static List<Assignee> GetSeedingAssignees()
            {
                List<Assignee> result = new List<Assignee>();
                Assignee a1 = new Assignee { UserName = "name1", PasswordHash = "password1" };
                result.Add(a1);
                Assignee a2 = new Assignee { UserName = "name2", PasswordHash = "password2" };
                result.Add(a2);

                return result;
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