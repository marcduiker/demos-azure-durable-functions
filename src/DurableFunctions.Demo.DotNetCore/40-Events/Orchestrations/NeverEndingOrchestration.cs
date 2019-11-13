using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace DurableFunctions.Demo.DotNetCore._40_Events.Orchestrations
{
    public class NeverEndingOrchestration
    {
        [FunctionName(nameof(NeverEndingOrchestration))]
        public async Task Run(
          [OrchestrationTrigger] IDurableOrchestrationContext context,
          ILogger logger)
        {
            await context.CreateTimer(
                context.CurrentUtcDateTime.AddHours(1), 
                CancellationToken.None);
            
            context.ContinueAsNew(null);
        }
    }
}
