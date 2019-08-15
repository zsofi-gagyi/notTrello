using Newtonsoft.Json;
using System;

namespace TodoWithDatabase.Models.DTOs
{
    public class ProjectDTO
    {
        [JsonProperty(PropertyName = "id", Order = 1)]
        public Guid Id { set; get; }

        [JsonProperty(PropertyName = "title", Order = 2)]
        public string Title { set; get; }

        [JsonProperty(PropertyName = "description", Order = 3)]
        public string Description { set; get; }
    }
}