using System.Threading.Tasks;
using DurableFunctions.Demo.DotNetCore.Basics.Activities;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

// ReSharper disable once CheckNamespace
namespace DurableFunctions.Demo.DotNetCore.Basics.Orchestrators
{
    public class HelloWorldOrchestrator
    {
        [FunctionName(nameof(HelloWorldOrchestrator))]
        public async Task<string> Run(
            [OrchestrationTrigger]IDurableOrchestrationContext context,
            ILogger log)
        {
            var result = await context.CallActivityAsync<string>(
                nameof(HelloWorldActivity),
                null);

            return result;
        }
    }
}
