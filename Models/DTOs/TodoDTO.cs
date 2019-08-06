using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoWithDatabase.Models.DTO
{
    public class TodoDTO
    {
        public long Id { set; get; }
        public string Title { set; get; }
        public bool Urgent { set; get; } = false;
        public bool Done { set; get; } = false;
    }
}
