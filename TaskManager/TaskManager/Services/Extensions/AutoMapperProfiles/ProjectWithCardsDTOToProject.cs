using AutoMapper;
using System;
using TaskManager.Models.DAOs;
using TaskManager.Models.DTOs;

namespace TaskManager.Services.Extensions.AutoMapperProfiles
{
    public class ProjectWithCardsDTOToProject : Profile
    {
        public ProjectWithCardsDTOToProject()
        {
            CreateMap<ProjectWithCardsDTO, Project>()
                .ForMember(dest => dest.Id,
                            opt => opt.MapFrom(p => Guid.Parse(p.Id)));
        }
    }
}