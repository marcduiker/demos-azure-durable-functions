using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DurableFunctions.Demo.DotNetCore.Chaining.Activities.Models;
using DurableTask.Core.Exceptions;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

// ReSharper disable once CheckNamespace
namespace DurableFunctions.Demo.DotNetCore.Chaining.Activities
{
    public class SearchCharacterActivity
    {
        private readonly HttpClient _httpClient;

        public SearchCharacterActivity(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [FunctionName(nameof(SearchCharacterActivity))]
        public async Task<Character> Run(
            [ActivityTrigger] string name,
            ILogger logger)
        {
            bool.TryParse(Environment.GetEnvironmentVariable("SkipRemoteSwapi"), out bool skipRemoteSwapi);
            string characterResult;
            if (skipRemoteSwapi)
            {
                characterResult = GetLocalPersonSearchResult();
            }
            else
            {
                characterResult = await GetRemotePersonSearchResult(name);
            }
            
            var characters = JToken.Parse(characterResult).SelectToken("results").ToObject<Character[]>();

            return characters.FirstOrDefault();
        }

        private async Task<string> GetRemotePersonSearchResult(string name)
        {
            var uri = $"{Environment.GetEnvironmentVariable("SwapiBaseUrl")}people?search={name}";
            var result = await _httpClient.GetAsync(uri);
            if (!result.IsSuccessStatusCode)
            {
                throw new TaskFailedException($"SwApi returned status code {result.StatusCode}");
            }

            var personContent = await result.Content.ReadAsStringAsync();

            return personContent;
        }

        private static string GetLocalPersonSearchResult()
        {
            return @"{""count"":1,""next"":null,""previous"":null,""results"":[{""name"":""!Local! Luke Skywalker"",""height"":""172"",""mass"":""77"",""hair_color"":""blond"",""skin_color"":""fair"",""eye_color"":""blue"",""birth_year"":""19BBY"",""gender"":""male"",""homeworld"":""https://swapi.co/api/planets/1/"",""films"":[""https://swapi.co/api/films/2/"",""https://swapi.co/api/films/6/"",""https://swapi.co/api/films/3/"",""https://swapi.co/api/films/1/"",""https://swapi.co/api/films/7/""],""species"":[""https://swapi.co/api/species/1/""],""vehicles"":[""https://swapi.co/api/vehicles/14/"",""https://swapi.co/api/vehicles/30/""],""starships"":[""https://swapi.co/api/starships/12/"",""https://swapi.co/api/starships/22/""],""created"":""2014-12-09T13:50:51.644000Z"",""edited"":""2014-12-20T21:17:56.891000Z"",""url"":""https://swapi.co/api/people/1/""}]}";
        }
    }
}
