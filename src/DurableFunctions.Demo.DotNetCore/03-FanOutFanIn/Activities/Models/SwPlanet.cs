using Newtonsoft.Json;

// ReSharper disable once CheckNamespace
namespace DurableFunctions.Demo.DotNetCore.FanOutFanIn.Orchestrations.Models
{
    public sealed class SwPlanet
    {
        public string Name { get; set; }

        [JsonProperty("residents")]
        public string[] ResidentUrls { get; set; }
    }
}