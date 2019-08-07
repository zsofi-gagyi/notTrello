using Castle.MicroKernel.SubSystems.Conversion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TodoWithDatabase.Models.DAOs.JoinTables;

namespace TodoWithDatabase.Models.DAOs
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
        public bool Done { set; get; } = false;

        public Project Project { set; get; }

        public List<AssigneeCard> AssigneeCards { set; get; } = new List<AssigneeCard>();
    }
}
