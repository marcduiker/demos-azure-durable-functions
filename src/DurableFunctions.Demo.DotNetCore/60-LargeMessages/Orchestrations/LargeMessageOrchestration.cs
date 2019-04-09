using System.Threading.Tasks;
using DurableFunctions.Demo.DotNetCore.LargeMessages.Activities;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

// ReSharper disable once CheckNamespace
namespace DurableFunctions.Demo.DotNetCore.LargeMessages.Orchestrations
{
    public class LargeMessageOrchestration
    {
        [FunctionName(nameof(LargeMessageOrchestration))]
        public async Task<string> Run(
            [OrchestrationTrigger]DurableOrchestrationContextBase context,
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
