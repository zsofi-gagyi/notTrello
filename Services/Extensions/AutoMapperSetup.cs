using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using TodoWithDatabase.Models.DAOs;
using TodoWithDatabase.Models.DTOs;

namespace TodoWithDatabase.Services.Extensions
{
    public static class AutoMapperSetup
    {
        
        public static void SetUpAutoMapper(this IServiceCollection services)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Assignee, AssigneeDTOSimple>();
                cfg.CreateMap<AssigneeToCreateDTO, Assignee>();
            });

            var mapper = config.CreateMapper();
            services.AddSingleton<IMapper>(mapper);
        }   
    }
}