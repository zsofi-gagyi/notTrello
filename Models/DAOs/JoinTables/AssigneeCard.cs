using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoWithDatabase.Models.DAOs.JoinTables
{
    public class AssigneeCard
    {
        public Assignee Assignee { get; set; }

        public Guid AssigneeId { get; set; }

        public Card Card { get; set; }

        public Guid CardId { get; set; }
    }
}
