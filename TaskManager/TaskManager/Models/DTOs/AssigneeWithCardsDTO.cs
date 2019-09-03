using Newtonsoft.Json;
using System.Collections.Generic;

namespace TaskManager.Models.DTOs
{
    public class AssigneeWithCardsDTO
    {
        [JsonProperty(PropertyName = "id", Order = 1)]
        public string Id { set; get; }

        [JsonProperty(PropertyName = "name", Order = 2)]
        public string UserName { set; get; }

        [JsonProperty(PropertyName = "role", Order = 3)]
        public string Role;

        [JsonProperty(PropertyName = "cards", Order = 4)]
        public List<CardWithProjectDTO> CardWithProjectsDTOs { set; get; }
    }
}