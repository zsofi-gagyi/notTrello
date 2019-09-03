using Newtonsoft.Json;

namespace TaskManager.Models.DTOs
{
    public class AssigneeToCreateDTO
    {
        [JsonProperty(PropertyName = "name", Required = Required.Always)]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "password", Required = Required.Always)]
        public string Password { get; set; }
    }
}
