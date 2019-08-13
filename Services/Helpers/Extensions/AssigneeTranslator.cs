using AutoMapper;
using TodoWithDatabase.Models;
using TodoWithDatabase.Models.DTO;

namespace TodoWithDatabase.Services.Extensions
{
    public static class AssigneeTranslator
    {
        /*
        public static AssigneeDTO ToDtoUsingMapper(this Assignee assignee, IMapper mapper)
        {
            AssigneeDTO result = new AssigneeDTO();
            result.Id = assignee.Id;
            result.Name = assignee.UserName;
            assignee.Todos.ForEach(t => result.Todos.Add(mapper.Map<TodoDTO>(t)));

            return result;
        }
        */
    }
}
