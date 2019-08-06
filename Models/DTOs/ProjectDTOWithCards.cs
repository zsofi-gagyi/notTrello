using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TodoWithDatabase.Models.DTOs
{
    public class ProjectDTOWithCards
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { set; get; }

        [JsonProperty(PropertyName = "title")]
        public string Title { set; get; }

        [JsonProperty(PropertyName = "description")]
        public string Description { set; get; }

        [JsonProperty(PropertyName = "responsibles")]
        public List<AssigneeDTO> AssigneeDTO { set; get; }

        [JsonProperty(PropertyName = "cards")]
        public List<CardDTOWithAssignees> CardDTOWithAssignees { set; get; }
    }
}