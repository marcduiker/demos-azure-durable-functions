using System.Linq;
using System.Threading.Tasks;
using DurableFunctions.Demo.DotNetCore.Chaining.Activities;
using DurableFunctions.Demo.DotNetCore.Chaining.Activities.Models;
using DurableFunctions.Demo.DotNetCore.Chaining.Orchestrations.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

// ReSharper disable once CheckNamespace
namespace DurableFunctions.Demo.DotNetCore.Chaining.Orchestrations
{
    public class GetCharacterInfoOrchestration
    {
        [FunctionName(nameof(GetCharacterInfoOrchestration))]
        public async Task<CharacterInfo> Run(
            [OrchestrationTrigger]DurableOrchestrationContextBase context,
            ILogger log)
        {
            var name = context.GetInput<string>();
            var result = new CharacterInfo();

            var characterResult = await context.CallActivityAsync<Character>(
                nameof(SearchCharacterActivity),
                name);
            result.Name = characterResult.Name;

            var homeWorldResult = await context.CallActivityAsync<string>(
                nameof(GetPlanetActivity),
                characterResult.PlanetUrl);
            result.HomeWorld = homeWorldResult;

            var speciesResult = await context.CallActivityAsync<Species>(
                nameof(GetSpeciesActivity),
                characterResult.SpeciesUrl.FirstOrDefault());
            result.Species = speciesResult.Name;

            var speciesHomeWorldResult = await context.CallActivityAsync<string>(
                nameof(GetPlanetActivity),
                speciesResult.HomeWorld);
            result.SpeciesHomeWorld = speciesHomeWorldResult;

            return result;
        }
    }
}
