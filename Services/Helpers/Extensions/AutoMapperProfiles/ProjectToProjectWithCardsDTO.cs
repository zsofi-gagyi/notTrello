using AutoMapper;
using AutoMapper.QueryableExtensions;
using System.Linq;
using TodoWithDatabase.Models.DAOs;
using TodoWithDatabase.Models.DTOs;

namespace TodoWithDatabase.App.Services.Helpers.Extensions.AutoMapperProfiles
{
    public class ProjectToProjectWithCardsDTO : Profile
    {
        public ProjectToProjectWithCardsDTO()
        {
            CreateMap<Project, ProjectWithCardsDTO>()
                .ForMember(dest => dest.CardWithAssigneeDTOs,
                            opt => opt.MapFrom(p => p.Cards))
                .ForMember(dest => dest.AssigneeDTOs,
                            opt => opt.MapFrom(c => c.AssigneeProjects.Select(ac => ac.Assignee)));
        }
    }
}