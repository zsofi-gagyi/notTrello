using System.Linq;
using AutoMapper;
using TodoWithDatabase.Models.DAOs;
using TodoWithDatabase.Models.DTO;

namespace TodoWithDatabase.App.Services.Helpers.Extensions.AutoMapperProfiles
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