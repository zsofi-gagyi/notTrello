using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using TaskManager.Services.Extensions.AutoMapperProfiles;
using TaskManager.Models.DAOs;
using TaskManager.Models.DTOs;

namespace TaskManager.Services.Extensions
{
    public static class AutoMapperSetup
    {
        public static void SetUpAutoMapper(this IServiceCollection services)
        {
            var mapper = CreateMapper();
            services.AddSingleton<IMapper>(mapper);
        }

        public static IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Assignee, AssigneeDTO>();
                cfg.AddProfile(new CardToCardWithAssigneesDTO());
                cfg.AddProfile(new ProjectToProjectWithCardsDTO());
                cfg.AddProfile(new AssigneeToAssigneeWithProjectsDTO());

                cfg.CreateMap<Project, ProjectDTO>(); 
                cfg.AddProfile(new CardToCardWithProjectsDTO());
                cfg.AddProfile(new AssigneeToAssigneeWithCardsDTO());

                cfg.CreateMap<AssigneeDTO, Assignee>();
                cfg.CreateMap<CardWithAssigneesDTO, Card>();
                cfg.CreateMap<ProjectWithCardsDTO, Project>();
            });

            var mapper = config.CreateMapper();
            return mapper;
        }
    }
}