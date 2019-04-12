using Newtonsoft.Json;

// ReSharper disable once CheckNamespace
namespace DurableFunctions.Demo.DotNetCore.FanOutFanIn.Orchestrators.Models
{
    public sealed class Planet
    {
        public string Name { get; set; }

        [JsonProperty("residents")]
        public string[] ResidentUrls { get; set; }
    }
}