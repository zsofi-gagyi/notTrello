using AutoMapper;
using System.Linq;
using TodoWithDatabase.Models.DAOs;
using TodoWithDatabase.Models.DTOs;

namespace TodoWithDatabase.App.Services.Helpers.Extensions.AutoMapperProfiles
{
    public class CardToCardWithProjectsDTO : Profile
    {
        public CardToCardWithProjectsDTO()
        {
            CreateMap<Card, CardWithProjectDTO>()
                .ForMember(dest => dest.ProjectDTO,
                            opt => opt.MapFrom(c => c.Project))
                .ForMember(dest => dest.AssigneeDTOs,
                            opt => opt.MapFrom(c => c.AssigneeCards.Select(ac => ac.Assignee)));
        }
    }
}