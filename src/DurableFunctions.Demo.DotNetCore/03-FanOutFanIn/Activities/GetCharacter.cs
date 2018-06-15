using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

// ReSharper disable once CheckNamespace
namespace DurableFunctions.Demo.DotNetCore.FanOutFanIn.Activities
{
    public static class GetCharacter
    {
        private static readonly HttpClient HttpClient = new HttpClient();

        [FunctionName(nameof(GetCharacter))]
        public static async Task<string> Run(
            [ActivityTrigger] string characterUri,
            ILogger logger)
        {
            var result = await HttpClient.GetAsync(characterUri);
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
