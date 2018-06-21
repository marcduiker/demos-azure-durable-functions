using System;
using System.Threading.Tasks;
using DurableFunctions.Demo.DotNetCore.Chaining.Activities;
using DurableFunctions.Demo.DotNetCore.Chaining.Activities.Models;
using DurableFunctions.Demo.DotNetCore.Chaining.Orchestrations.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

// ReSharper disable once CheckNamespace
namespace DurableFunctions.Demo.DotNetCore.Chaining.Orchestrations
{
    public static class GetCharacterInfo
    {
        [FunctionName(nameof(GetCharacterInfo))]
        public static async Task<CharacterInfo> Run(
            [OrchestrationTrigger]DurableOrchestrationContextBase context,
            ILogger log)
        {
            var name = context.GetInput<string>();

            var result = new CharacterInfo();

            var characterResult = await context.CallActivityAsync<Character>(
                nameof(SearchCharacter),
                name);
            
            if (characterResult != null)
            {
                result.Name = characterResult.Name;

                var planetResult = await context.CallActivityAsync<string>(
                    nameof(GetPlanet),
                    characterResult.PlanetUrl);

                result.HomeWorld = planetResult;
            }

            return result;
        }
    }
}
