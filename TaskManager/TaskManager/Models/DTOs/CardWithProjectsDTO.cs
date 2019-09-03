using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TaskManager.Models.DTOs
{
    public class CardWithProjectDTO
    {
        [JsonProperty(PropertyName = "id", Order = 1)]
        public string Id { set; get; }

        [JsonProperty(PropertyName = "title", Order = 2)]
        public string Title { set; get; }

        [JsonProperty(PropertyName = "description", Order = 3)]
        public string Description { set; get; }

        [JsonProperty(PropertyName = "deadline", Order = 4)]
        public DateTime Deadline { set; get; }

        [JsonProperty(PropertyName = "done", Order = 5)]
        public bool Done { set; get; } = false;

        [JsonProperty(PropertyName = "project", Order = 6)]
        public ProjectDTO ProjectDTO;

        [JsonProperty(PropertyName = "responsibles", Order = 7)]
        public List<AssigneeDTO> AssigneeDTOs;
    }
}