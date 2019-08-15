using AutoMapper;
using System;
using TodoWithDatabase.Models.DAOs;
using TodoWithDatabase.Models.DTOs;

namespace TodoWithDatabase.App.Services.Helpers.Extensions.AutoMapperProfiles
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