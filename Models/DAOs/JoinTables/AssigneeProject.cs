using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoWithDatabase.Models.DAOs.JoinTables
{
    public class AssigneeProject
    {
        public Assignee Assignee { get; set; }

        public Guid AssigneeId { get; set; }

        public Project Project { get; set; }

        public Guid ProjectId { get; set; }
    }
}
