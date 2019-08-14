using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoWithDatabase.Models.DAOs.JoinTables
{
    public class AssigneeCard
    {
        public Assignee Assignee { get; set; }

        public string AssigneeId { get; set; }

        public Card Card { get; set; }

        public Guid CardId { get; set; }

        public AssigneeCard(Assignee assignee, Card card)
        {
            this.Assignee = assignee;
            this.AssigneeId = assignee.Id;
            this.Card = card;
            this.CardId = card.Id;
        }

        public AssigneeCard()
        {
        }
    }
}
