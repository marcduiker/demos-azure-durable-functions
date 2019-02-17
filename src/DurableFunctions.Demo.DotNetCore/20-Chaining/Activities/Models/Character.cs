using Newtonsoft.Json;

// ReSharper disable once CheckNamespace
namespace DurableFunctions.Demo.DotNetCore.Chaining.Activities.Models
{
    public sealed class Character
    {
        public string Name { get; set; }

        [JsonProperty("url")]
        public string CharacterUrl { get; set; }

        [JsonProperty("homeworld")]
        public string PlanetUrl { get; set; }
    }
}