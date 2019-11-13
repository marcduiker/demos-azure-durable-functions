using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// ReSharper disable once CheckNamespace
namespace DurableFunctions.Demo.DotNetCore.Chaining.Activities
{
    public class GetPlanetActivity
    {
        private readonly HttpClient _httpClient;

        public GetPlanetActivity(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [FunctionName(nameof(GetPlanetActivity))]
        public async Task<string> Run(
            [ActivityTrigger] string planetUri,
            ILogger logger)
        {
            bool.TryParse(Environment.GetEnvironmentVariable("SkipRemoteSwapi"), out bool skipRemoteSwapi);
            dynamic planetResult;
            if (skipRemoteSwapi)
            {
                planetResult = GetLocalPlanetResult();
            }
            else
            {
                planetResult = await GetRemotePlanetResult(planetUri);
            }
            
            return planetResult.name;
        }

        private async Task<object> GetRemotePlanetResult(string planetUri)
        {
            var result = await _httpClient.GetAsync(planetUri);
            if (!result.IsSuccessStatusCode)
            {
                return null;
            }

            var planetContent = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject(planetContent);
        }

        private static JToken GetLocalPlanetResult()
        {
            return JToken.Parse(@"{""name"":""!Local! Tatooine"",""rotation_period"":""23"",""orbital_period"":""304"",""diameter"":""10465"",""climate"":""arid"",""gravity"":""1 standard"",""terrain"":""desert"",""surface_water"":""1"",""population"":""200000"",""residents"":[""https://swapi.co/api/people/1/"",""https://swapi.co/api/people/2/"",""https://swapi.co/api/people/4/"",""https://swapi.co/api/people/6/"",""https://swapi.co/api/people/7/"",""https://swapi.co/api/people/8/"",""https://swapi.co/api/people/9/"",""https://swapi.co/api/people/11/"",""https://swapi.co/api/people/43/"",""https://swapi.co/api/people/62/""],""films"":[""https://swapi.co/api/films/5/"",""https://swapi.co/api/films/4/"",""https://swapi.co/api/films/6/"",""https://swapi.co/api/films/3/"",""https://swapi.co/api/films/1/""],""created"":""2014-12-09T13:50:49.641000Z"",""edited"":""2014-12-21T20:48:04.175778Z"",""url"":""https://swapi.co/api/planets/1/""}");
        }
    }
}
