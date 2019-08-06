using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace TodoWithDatabase.Models
{
    public class Assignee : IdentityUser
    {
        public List<Todo> Todos { set; get; }

        public Assignee()
        {
            this.Todos = new List<Todo>();
        }
    }
}
