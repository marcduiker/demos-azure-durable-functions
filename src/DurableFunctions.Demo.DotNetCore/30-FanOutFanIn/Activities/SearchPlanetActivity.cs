using System;
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
    public class SearchPlanetActivity
    {
        [FunctionName(nameof(SearchPlanetActivity))]
        public async Task<Planet> Run(
            [ActivityTrigger] string name,
            ILogger logger)
        {
            string planetResult;
            bool.TryParse(Environment.GetEnvironmentVariable("SkipRemoteSwapi"), out bool skipRemoteSwapi);
            if (skipRemoteSwapi)
            {
                planetResult = GetLocalPlanetSearchResult();
            }
            else
            {
                planetResult = await GetRemotePlanetSearchResult(name);
            }
            
            var planets = JToken.Parse(planetResult).SelectToken("results").ToObject<Planet[]>();

            return planets.FirstOrDefault();
        }

        private async Task<string> GetRemotePlanetSearchResult(string name)
        {
            var uri = $"{Environment.GetEnvironmentVariable("SwapiBaseUrl")}planets?search={name}";
            var result = await httpClient.GetAsync(uri);
            if (!result.IsSuccessStatusCode)
            {
                return null;
            }

            var planetContent = await result.Content.ReadAsStringAsync();

            return planetContent;
        }

        private static string GetLocalPlanetSearchResult()
        {
            return @"{""count"":1,""next"":null,""previous"":null,""results"":[{""name"":""!Local! Tatooine"",""rotation_period"":""23"",""orbital_period"":""304"",""diameter"":""10465"",""climate"":""arid"",""gravity"":""1 standard"",""terrain"":""desert"",""surface_water"":""1"",""population"":""200000"",""residents"":[""https://swapi.co/api/people/1/"",""https://swapi.co/api/people/2/"",""https://swapi.co/api/people/4/"",""https://swapi.co/api/people/6/"",""https://swapi.co/api/people/7/"",""https://swapi.co/api/people/8/"",""https://swapi.co/api/people/9/"",""https://swapi.co/api/people/11/"",""https://swapi.co/api/people/43/"",""https://swapi.co/api/people/62/""],""films"":[""https://swapi.co/api/films/5/"",""https://swapi.co/api/films/4/"",""https://swapi.co/api/films/6/"",""https://swapi.co/api/films/3/"",""https://swapi.co/api/films/1/""],""created"":""2014-12-09T13:50:49.641000Z"",""edited"":""2014-12-21T20:48:04.175778Z"",""url"":""https://swapi.co/api/planets/1/""}]}";
        }

        private readonly HttpClient httpClient = new HttpClient();
    }
}
