using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DurableFunctions.Demo.DotNetCore.FanOutFanIn.Orchestrations.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

// ReSharper disable once CheckNamespace
namespace DurableFunctions.Demo.DotNetCore.FanOutFanIn.Activities
{
    
    public static class SearchPlanet
    {
        private const string SwApiPlanet = "https://swapi.co/api/planets/";
        private static readonly HttpClient HttpClient = new HttpClient();

        [FunctionName(nameof(SearchPlanet))]
        public static async Task<Planet> Run(
            [ActivityTrigger] string name,
            ILogger logger)
        {

            var uri = $"{SwApiPlanet}?search={name}";
            var result = await HttpClient.GetAsync(uri);
            if (!result.IsSuccessStatusCode)
            {
                return null;
            }

            var contentResult = result.Content.ReadAsStringAsync().Result;
            var character = JToken.Parse(contentResult).SelectToken("results").ToObject<Planet[]>();

            return character.FirstOrDefault();
        }
    }
}
