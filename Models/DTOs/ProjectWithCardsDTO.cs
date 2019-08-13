using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TodoWithDatabase.Models.DTOs
{
    public class ProjectWithCardsDTO
    {
        [JsonProperty(PropertyName = "id", Order = 1)]
        public string Id { set; get; }

        [JsonProperty(PropertyName = "title", Order = 2)]
        public string Title { set; get; }

        [JsonProperty(PropertyName = "description", Order = 3)]
        public string Description { set; get; }

        [JsonProperty(PropertyName = "responsibles", Order = 4)]
        public List<AssigneeDTO> AssigneeDTOs { set; get; }

        [JsonProperty(PropertyName = "cards", Order = 5)]
        public List<CardWithAssigneesDTO> CardWithAssigneeDTOs { set; get; }
    }
}