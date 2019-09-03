using AutoMapper;
using System.Linq;
using TaskManager.Models.DAOs;
using TaskManager.Models.DTOs;

namespace TaskManager.Services.Extensions.AutoMapperProfiles
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