using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TodoWithDatabase.Models
{
    public class Todo
    {
        public long Id { set; get; }
        public string Title { set; get; }
        [Column(TypeName = "SMALLINT")]
        public bool Urgent { set; get; } = false;
        [Column(TypeName = "SMALLINT")]
        public bool Done { set; get; } = false;
        public Assignee Assignee { set; get; }

        public Todo()
        {
        }

        public Todo(string title, Assignee assignee) 
        {
            this.Assignee = assignee;
            this.Title = title;
        }
    }
}
