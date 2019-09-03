using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Models.DAOs;
using TaskManager.Models.DTOs;

namespace TaskManager.Services.Interfaces
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
