using System.Collections.Generic;
using System.Threading.Tasks;
using TodoWithDatabase.Models;
using TodoWithDatabase.Models.DAOs;
using TodoWithDatabase.Models.DTO;
using TodoWithDatabase.Models.DTOs;

namespace TodoWithDatabase.Services.Interfaces
{
    public interface IAssigneeService
    {
        Assignee GetWithAssigneeCards(string name);

        Task<Assignee> CreateAndReturnNewAsync(AssigneeToCreateDTO assigneeDTO);

        Task CreateAndSignInAsync(string name, string password);

        Task CreateAndSignInWithEmailAsync(string name, string email);

        void Update(Assignee assignee);

        List<AssigneeDTO> GetAndTranslateAll();

        Task<AssigneeWithProjectsDTO> GetAndTranslateToAssigneWithProjectsDTOAsync(string userId);

        Task<AssigneeWithCardsDTO> GetAndTranslateToAssigneWithCardsDTOAsync(string userId);
    }
}
