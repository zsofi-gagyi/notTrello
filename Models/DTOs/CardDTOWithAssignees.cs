using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoWithDatabase.Models.DTOs
{
    public class CardDTOWithAssignees
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { set; get; }

        [JsonProperty(PropertyName = "title")]
        public string Title { set; get; }

        [JsonProperty(PropertyName = "title")]
        public string Description { set; get; }

        [JsonProperty(PropertyName = "deadline")]
        public DateTime Deadline { set; get; }

        [JsonProperty(PropertyName = "done")]
        public bool Done { set; get; } = false;

        [JsonProperty(PropertyName = "responsibles")]
        public List<AssigneeDTO> AssigneeDTOs;
    }
}
