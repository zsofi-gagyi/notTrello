using System.Collections.Generic;
using TodoWithDatabase.Repository;
using TodoWithDatabase.Models;

namespace TodoWithDatabase.IntegrationTests.Helpers
{
    public static class Utilities
    {
        #region snippet1
        public static void InitializeDbForTests(MyContext db)
        {
            List<Assignee> assignees = GetSeedingAssignees();
            db.Assignees.AddRange(assignees);
            db.Todos.AddRange(GetSeedingTodos(assignees));
            db.SaveChanges();
        }

        public static List<Assignee> GetSeedingAssignees()
        {
            List<Assignee> result = new List<Assignee>();
            Assignee a1 = new Assignee { UserName = "name1", PasswordHash = "password1" , Id = "1"};
            result.Add(a1);
            Assignee a2 = new Assignee { UserName = "name2", PasswordHash = "password2", Id = "2" };
            result.Add(a2);

            return result;
        }

        public static List<Todo> GetSeedingTodos(List<Assignee> assignees)
        {
            List<Todo> result = new List<Todo>();
            Todo t1 = new Todo("task1", assignees[0]);
            t1.Id = 3;
            Todo t2 = new Todo("task2", assignees[1]);
            t2.Id = 4;

            return result;
        }
        #endregion
    }
}