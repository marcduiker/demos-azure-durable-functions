using System.Threading.Tasks;
using DurableFunctions.Demo.DotNetCore.LargeMessages.Activities;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

// ReSharper disable once CheckNamespace
namespace DurableFunctions.Demo.DotNetCore.LargeMessages.Orchestrators
{
    public class LargeMessageOrchestrator
    {
        [FunctionName(nameof(LargeMessageOrchestrator))]
        public async Task<string> Run(
            [OrchestrationTrigger]IDurableOrchestrationContext context,
            ILogger log)
        {
            var nrOfChars = context.GetInput<int>();

            var result = await context.CallActivityAsync<string>(
                nameof(LargeMessageActivity),
                nrOfChars);

            return result;
        }
    }
}
