using AutoMapper;
using System.Linq;
using TodoWithDatabase.Models.DAOs;
using TodoWithDatabase.Models.DTOs;

namespace TodoWithDatabase.App.Services.Helpers.Extensions.AutoMapperProfiles
{
    public class CardToCardWithAssigneesDTO : Profile
    {
        public CardToCardWithAssigneesDTO()
        {
            CreateMap<Card, CardWithAssigneesDTO>()
                .ForMember(dest => dest.AssigneeDTOs, 
                            opt => opt.MapFrom(c => c.AssigneeCards.Select(ac => ac.Assignee)));
        }
    }
}
