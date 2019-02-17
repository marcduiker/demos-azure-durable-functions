using System.Threading.Tasks;
using DurableFunctions.Demo.DotNetCore.Basics.Activities;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

// ReSharper disable once CheckNamespace
namespace DurableFunctions.Demo.DotNetCore.Basics.Orchestrations
{
    public static class FailingOrchestration
    {
        [FunctionName(nameof(FailingOrchestration))]
        public static async Task<string> Run(
            [OrchestrationTrigger]DurableOrchestrationContextBase context,
            ILogger log)
        {
            var result = await context.CallActivityAsync<string>(
                nameof(FailingActivity),
                string.Empty);

            return result;
        }
    }
}
