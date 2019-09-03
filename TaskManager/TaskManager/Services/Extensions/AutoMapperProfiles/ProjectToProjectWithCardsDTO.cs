using AutoMapper;
using System.Linq;
using TaskManager.Models.DAOs;
using TaskManager.Models.DTOs;

namespace TaskManager.Services.Extensions.AutoMapperProfiles
{ 
    public class ProjectToProjectWithCardsDTO : Profile
    {
        public ProjectToProjectWithCardsDTO()
        {
            CreateMap<Project, ProjectWithCardsDTO>()
                .ForMember(dest => dest.CardWithAssigneesDTOs,
                            opt => opt.MapFrom(p => p.Cards))
                .ForMember(dest => dest.AssigneeDTOs,
                            opt => opt.MapFrom(c => c.AssigneeProjects.Select(ac => ac.Assignee)));
        }
    }
}