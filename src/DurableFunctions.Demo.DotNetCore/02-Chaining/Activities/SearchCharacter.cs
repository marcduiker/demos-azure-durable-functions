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
    
    public static class SearchCharacter
    {
        private const string SwApiPeople = "https://swapi.co/api/people/";
        private static readonly HttpClient HttpClient = new HttpClient();

        [FunctionName(nameof(SearchCharacter))]
        public static async Task<Character> Run(
            [ActivityTrigger] string name,
            ILogger logger)
        {

            var uri = $"{SwApiPeople}?search={name}";
            var result = await HttpClient.GetAsync(uri);
            if (!result.IsSuccessStatusCode)
            {
                throw new TaskFailedException($"SwApi returned status code {result.StatusCode}");
            }

            var contentResult = result.Content.ReadAsStringAsync().Result;
            var character = JToken.Parse(contentResult).SelectToken("results").ToObject<Character[]>();

            return character.FirstOrDefault();
        }
    }
}
