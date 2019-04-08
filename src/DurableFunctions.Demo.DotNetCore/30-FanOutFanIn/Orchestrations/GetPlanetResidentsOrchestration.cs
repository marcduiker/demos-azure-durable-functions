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
    public class GetPlanetResidentsOrchestration
    {
        [FunctionName(nameof(GetPlanetResidentsOrchestration))]
        public async Task<PlanetResidents> Run(
            [OrchestrationTrigger]DurableOrchestrationContextBase context,
            ILogger log)
        {
            var planetName = context.GetInput<string>();

            var result = new PlanetResidents();

            var planetResult = await context.CallActivityAsync<Planet>(
                nameof(SearchPlanetActivity),
                planetName);

            if (planetResult != null)
            {
                result.PlanetName = planetResult.Name;

                var tasks = new List<Task<string>>();
                foreach (var residentUrl in planetResult.ResidentUrls)
                {
                    tasks.Add(
                        context.CallActivityAsync<string>(
                            nameof(GetCharacterActivity),
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
