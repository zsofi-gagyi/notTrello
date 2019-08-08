using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TodoWithDatabase.Models.DAOs.JoinTables;

namespace TodoWithDatabase.Models.DAOs
{
    public class Project
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { set; get; }

        public string Title { set; get; }

        public string Description { set; get; }

        public List<AssigneeProject> AssigneeProjects { set; get; }

        public List<Card> Cards { set; get; }

        public Project() // necessary for model binding
        {
            AssigneeProjects = new List<AssigneeProject>();
            Cards = new List<Card>();
        }
    }
}
