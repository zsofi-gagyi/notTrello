using System.Linq;
using AutoMapper;
using TodoWithDatabase.Models.DAOs;
using TodoWithDatabase.Models.DTO;

namespace TodoWithDatabase.App.Services.Helpers.Extensions.AutoMapperProfiles
{
    public class AssigneeToAssigneeWithProjectsDTO : Profile
    {
        public AssigneeToAssigneeWithProjectsDTO()
        {
            CreateMap<Assignee, AssigneeWithProjectsDTO>()
              .ForMember(dest => dest.ProjectWithCardsDTOs,
                          opt => opt.MapFrom(a => a.AssigneeProjects.Select(ap => ap.Project)))
              .ForMember(dest => dest.Role,
                          opt => opt.MapFrom(a => a));
        }
    }
}
