using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DurableFunctions.Demo.DotNetCore.FanOutFanIn.Activities;
using DurableFunctions.Demo.DotNetCore.FanOutFanIn.Orchestrations.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

// ReSharper disable once CheckNamespace
namespace DurableFunctions.Demo.DotNetCore.FanOutFanIn.Orchestrations
{
    public static class GetSwPlanetResidents
    {
        [FunctionName(nameof(GetSwPlanetResidents))]
        public static async Task<SwPlanetResidents> Run(
            [OrchestrationTrigger]DurableOrchestrationContextBase context,
            ILogger log)
        {
            var name = context.GetInput<string>();

            var result = new SwPlanetResidents();

            var planetResult = await context.CallActivityAsync<SwPlanet>(
                nameof(SearchPlanet),
                name);

            if (planetResult != null)
            {
                result.PlanetName = planetResult.Name;

                var tasks = new List<Task<string>>();
                foreach (var residentUrl in planetResult.ResidentUrls)
                {
                    tasks.Add(
                        context.CallActivityAsync<string>(
                            nameof(GetCharacter),
                            residentUrl)
                    );
                }

                await Task.WhenAll(tasks);

                result.Residents = tasks.Select(task => task.Result).ToArray();
            }

            return result;
        }
    }
}
