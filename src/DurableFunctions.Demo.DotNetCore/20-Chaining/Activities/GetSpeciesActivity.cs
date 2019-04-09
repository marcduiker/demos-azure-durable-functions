using System;
using System.Net.Http;
using System.Threading.Tasks;
using DurableFunctions.Demo.DotNetCore.Chaining.Activities.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// ReSharper disable once CheckNamespace
namespace DurableFunctions.Demo.DotNetCore.Chaining.Activities
{
    public class GetSpeciesActivity
    {
        [FunctionName(nameof(GetSpeciesActivity))]
        public async Task<Species> Run(
            [ActivityTrigger] string speciesUri,
            ILogger logger)
        {
            bool.TryParse(Environment.GetEnvironmentVariable("SkipRemoteSwapi"), out bool skipRemoteSwapi);
            Species speciesResult;
            if (skipRemoteSwapi)
            {
                speciesResult = GetLocalSpeciesResult();
            }
            else
            {
                speciesResult = await GetRemoteSpeciesResult(speciesUri);
            }
            
            return speciesResult;
        }

        private async Task<Species> GetRemoteSpeciesResult(string speciesUri)
        {
            var result = await httpClient.GetAsync(speciesUri);
            if (!result.IsSuccessStatusCode)
            {
                return null;
            }

            var speciesContent = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Species>(speciesContent);
        }

        private static Species GetLocalSpeciesResult()
        {
            var token = JToken.Parse(@"{""name"":""!Local! Human"",""classification"":""mammal"",""designation"":""sentient"",""average_height"":""180"",""skin_colors"":""caucasian, black, asian, hispanic"",""hair_colors"":""blonde, brown, black, red"",""eye_colors"":""brown, blue, green, hazel, grey, amber"",""average_lifespan"":""120"",""homeworld"":""https://swapi.co/api/planets/9/"",""language"":""Galactic Basic"",""people"":[""https://swapi.co/api/people/1/"",""https://swapi.co/api/people/4/"",""https://swapi.co/api/people/5/"",""https://swapi.co/api/people/6/"",""https://swapi.co/api/people/7/"",""https://swapi.co/api/people/9/"",""https://swapi.co/api/people/10/"",""https://swapi.co/api/people/11/"",""https://swapi.co/api/people/12/"",""https://swapi.co/api/people/14/"",""https://swapi.co/api/people/18/"",""https://swapi.co/api/people/19/"",""https://swapi.co/api/people/21/"",""https://swapi.co/api/people/22/"",""https://swapi.co/api/people/25/"",""https://swapi.co/api/people/26/"",""https://swapi.co/api/people/28/"",""https://swapi.co/api/people/29/"",""https://swapi.co/api/people/32/"",""https://swapi.co/api/people/34/"",""https://swapi.co/api/people/43/"",""https://swapi.co/api/people/51/"",""https://swapi.co/api/people/60/"",""https://swapi.co/api/people/61/"",""https://swapi.co/api/people/62/"",""https://swapi.co/api/people/66/"",""https://swapi.co/api/people/67/"",""https://swapi.co/api/people/68/"",""https://swapi.co/api/people/69/"",""https://swapi.co/api/people/74/"",""https://swapi.co/api/people/81/"",""https://swapi.co/api/people/84/"",""https://swapi.co/api/people/85/"",""https://swapi.co/api/people/86/"",""https://swapi.co/api/people/35/""],""films"":[""https://swapi.co/api/films/2/"",""https://swapi.co/api/films/7/"",""https://swapi.co/api/films/5/"",""https://swapi.co/api/films/4/"",""https://swapi.co/api/films/6/"",""https://swapi.co/api/films/3/"",""https://swapi.co/api/films/1/""],""created"":""2014-12-10T13:52:11.567000Z"",""edited"":""2015-04-17T06:59:55.850671Z"",""url"":""https://swapi.co/api/species/1/""}");
            return token.ToObject<Species>();
        }

        private static readonly HttpClient httpClient = new HttpClient();

    }
}
