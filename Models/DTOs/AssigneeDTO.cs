using Newtonsoft.Json;

namespace TodoWithDatabase.Models
{
    public class AssigneeDTO
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { set; get; }

        [JsonProperty(PropertyName = "name")]
        public string UserName { set; get; }
    }
}
