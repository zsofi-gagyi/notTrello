using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using TaskManager.Models.DAOs.JoinTables;

namespace TaskManager.Models.DAOs
{
    public class Assignee : IdentityUser
    {
        public List<AssigneeProject> AssigneeProjects { set; get; }

        public List<AssigneeCard> AssigneeCards { set; get; }

        public Assignee()
        {
            AssigneeProjects = new List<AssigneeProject>();
            AssigneeCards = new List<AssigneeCard>();
        }
    }
}
