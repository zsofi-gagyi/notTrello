using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using TodoWithDatabase.Models.DAOs;
using TodoWithDatabase.Models.DAOs.JoinTables;

namespace TodoWithDatabase.Models
{
    public class Assignee : IdentityUser
    {
        public List<AssigneeProject> AssigneeProjects { set; get; }

        public List<AssigneeCard> AssigneeCards { set; get; }
    }
}
