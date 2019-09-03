using AutoMapper;
using System.Linq;
using TaskManager.Models.DAOs;
using TaskManager.Models.DTOs;

namespace TaskManager.Services.Extensions.AutoMapperProfiles
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
