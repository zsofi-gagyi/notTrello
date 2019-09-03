using Newtonsoft.Json;

namespace TaskManager.Models.DTOs
{
    public class AssigneeDTO
    {
        [JsonProperty(PropertyName = "id", Order = 1)]
        public string Id { set; get; }

        [JsonProperty(PropertyName = "name", Order = 2)]
        public string UserName { set; get; }
    }
}
