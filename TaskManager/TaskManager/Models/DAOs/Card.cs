using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskManager.Models.DAOs.JoinTables;

namespace TaskManager.Models.DAOs
{
    public class Card 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { set; get; }

        public string Title { set; get; }

        public string Description { set; get; }

        public DateTime Deadline { set; get; }

        [Column(TypeName = "SMALLINT")]
        public bool Done { set; get; }

        public Project Project { set; get; }

        public List<AssigneeCard> AssigneeCards { set; get; }

        public Card() 
        {
            Done = false;
            AssigneeCards = new List<AssigneeCard>();
        }
    }
}
