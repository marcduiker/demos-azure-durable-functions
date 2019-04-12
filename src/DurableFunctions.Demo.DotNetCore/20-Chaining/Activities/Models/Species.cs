using Newtonsoft.Json;

namespace DurableFunctions.Demo.DotNetCore.Chaining.Activities.Models
{
    public class Species
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("homeworld")]
        public string HomeWorld { get; set; }
    }
}
