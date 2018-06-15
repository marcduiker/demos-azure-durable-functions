using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

// ReSharper disable once CheckNamespace
namespace DurableFunctions.Demo.DotNetCore.Chaining.Activities
{
    public static class GetPlanet
    {
        private static readonly HttpClient HttpClient = new HttpClient();

        [FunctionName(nameof(GetPlanet))]
        public static async Task<string> Run(
            [ActivityTrigger] string planetUri,
            ILogger logger)
        {
            var result = await HttpClient.GetAsync(planetUri);
            if (!result.IsSuccessStatusCode)
            {
                return null;
            }

            dynamic contentResult = JsonConvert.DeserializeObject(
                result.Content.ReadAsStringAsync().Result);
            
            return contentResult.name;
        }
    }
}
