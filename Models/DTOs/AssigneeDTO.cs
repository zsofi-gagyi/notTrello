using System;
using System.Collections.Generic;
using System.Linq;
using TodoWithDatabase.Models.DTO;



namespace TodoWithDatabase.Models
{
    public class AssigneeDTO
    {
        public string Id { set; get; }
        public string Name { set; get; }
        public List<TodoDTO> Todos { set; get; }

        public AssigneeDTO()
        {
            this.Todos = new List<TodoDTO>();
        }
    }
}
