using System.Threading.Tasks;
using DurableFunctions.Demo.DotNetCore.Basics.Activities;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

// ReSharper disable once CheckNamespace
namespace DurableFunctions.Demo.DotNetCore.Basics.Orchestrators
{
    public class HelloNameOrchestrator
    {
        [FunctionName(nameof(HelloNameOrchestrator))]
        public async Task<string> Run(
            [OrchestrationTrigger]IDurableOrchestrationContext context,
            ILogger log)
        {
            var name = context.GetInput<string>();

            var result = await context.CallActivityAsync<string>(
                nameof(HelloNameActivity), 
                name);

            return result;
        }
    }
}
