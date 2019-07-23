using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// ReSharper disable once CheckNamespace
namespace DurableFunctions.Demo.DotNetCore.FanOutFanIn.Activities
{
    public class GetCharacterActivity
    {
        public GetCharacterActivity(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [FunctionName(nameof(GetCharacterActivity))]
        public async Task<string> Run(
            [ActivityTrigger] string characterUri,
            ILogger logger)
        {
            dynamic characterResult;
            bool.TryParse(Environment.GetEnvironmentVariable("SkipRemoteSwapi"), out bool skipRemoteSwapi);
            if (skipRemoteSwapi)
            {
                characterResult = GetLocalCharacterResult();
            }
            else
            {
                characterResult = await GetRemoteCharacterResult(characterUri);
            }
            
            return characterResult.name;
        }

        private async Task<object> GetRemoteCharacterResult(string characterUri)
        {
            var result = await _httpClient.GetAsync(characterUri);
            if (!result.IsSuccessStatusCode)
            {
                return null;
            }

            var characterContent = await result.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject(characterContent);
        }

        private static JToken GetLocalCharacterResult()
        {
            return JToken.Parse(@"{""name"":""!Local! Luke Skywalker"",""height"":""172"",""mass"":""77"",""hair_color"":""blond"",""skin_color"":""fair"",""eye_color"":""blue"",""birth_year"":""19BBY"",""gender"":""male"",""homeworld"":""https://swapi.co/api/planets/1/"",""films"":[""https://swapi.co/api/films/2/"",""https://swapi.co/api/films/6/"",""https://swapi.co/api/films/3/"",""https://swapi.co/api/films/1/"",""https://swapi.co/api/films/7/""],""species"":[""https://swapi.co/api/species/1/""],""vehicles"":[""https://swapi.co/api/vehicles/14/"",""https://swapi.co/api/vehicles/30/""],""starships"":[""https://swapi.co/api/starships/12/"",""https://swapi.co/api/starships/22/""],""created"":""2014-12-09T13:50:51.644000Z"",""edited"":""2014-12-20T21:17:56.891000Z"",""url"":""https://swapi.co/api/people/1/""}");
        }

        private readonly HttpClient _httpClient;
    }
}
