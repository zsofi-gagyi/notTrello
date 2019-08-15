using System.Collections.Generic;
using TodoWithDatabase.Models;
using TodoWithDatabase.Models.DAOs;
using TodoWithDatabase.Models.DTO;
using TodoWithDatabase.Models.DTOs;

namespace TodoWithDatabase.Services.Interfaces
{
    public interface IAssigneeService
    {
        Assignee GetWithAssigneeCards(string name);

        Assignee CreateAndReturnNew(AssigneeToCreateDTO assigneeDTO);

        void CreateAndSignIn(string name, string password);

        void Update(Assignee assignee);

        List<AssigneeDTO> GetAndTranslateAll();

        AssigneeWithProjectsDTO GetAndTranslateToAssigneWithProjectsDTO(string userId);

        AssigneeWithCardsDTO GetAndTranslateToAssigneWithCardsDTO(string userId);
    }
}
