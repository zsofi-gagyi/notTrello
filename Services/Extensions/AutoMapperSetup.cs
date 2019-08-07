using TodoWithDatabase.Models;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using TodoWithDatabase.Models.DTO;

namespace TodoWithDatabase.Services.Extensions
{
    public static class AutoMapperSetup
    {
        
        public static void setUpAutoMapper(this IServiceCollection services)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Assignee, AssigneeDTOSimple>()); 
            var mapper = config.CreateMapper();
            services.AddSingleton(mapper);

        }   
    }
}