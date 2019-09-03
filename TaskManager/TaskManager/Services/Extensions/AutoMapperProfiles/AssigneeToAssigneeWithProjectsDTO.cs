using System.Linq;
using AutoMapper;
using TaskManager.Models.DAOs;
using TaskManager.Models.DTOs;

namespace TaskManager.Services.Extensions.AutoMapperProfiles
{
    public class AssigneeToAssigneeWithProjectsDTO : Profile
    {
        public AssigneeToAssigneeWithProjectsDTO()
        {
            CreateMap<Assignee, AssigneeWithProjectsDTO>()
              .ForMember(dest => dest.ProjectWithCardsDTOs,
                          opt => opt.MapFrom(a => a.AssigneeProjects.Select(ap => ap.Project)));
        }
    }
}
