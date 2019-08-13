using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using TodoWithDatabase.App.Services.Helpers.Extensions.AutoMapperProfiles;
using TodoWithDatabase.Models;
using TodoWithDatabase.Models.DAOs;

namespace TodoWithDatabase.Services.Extensions
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
                cfg.AddProfile(new CardToCardWithAssigneeDTOs());
                cfg.AddProfile(new ProjectToProjectWithCardsDTO());
                cfg.AddProfile(new AssigneeToAssigneeWithProjectsDTO());
            });

            var mapper = config.CreateMapper();
            return mapper;
        }
    }
}