using System.Linq;
using AutoMapper;
using TaskManager.Models.DAOs;
using TaskManager.Models.DTOs;

namespace TaskManager.Services.Extensions.AutoMapperProfiles
{
    public class AssigneeToAssigneeWithCardsDTO : Profile
    {
        public AssigneeToAssigneeWithCardsDTO()
        {
            CreateMap<Assignee, AssigneeWithCardsDTO>()
              .ForMember(dest => dest.CardWithProjectsDTOs,
                          opt => opt.MapFrom(a => a.AssigneeCards.Select(ac => ac.Card)));
        }
    }
}