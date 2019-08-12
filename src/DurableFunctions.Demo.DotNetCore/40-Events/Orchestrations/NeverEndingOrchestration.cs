using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace DurableFunctions.Demo.DotNetCore._40_Events.Orchestrations
{
    public class NeverEndingOrchestration
    {
        [FunctionName(nameof(NeverEndingOrchestration))]
        public async Task Run(
          [OrchestrationTrigger] DurableOrchestrationContextBase context,
          ILogger logger)
        {
            await context.CreateTimer(
                context.CurrentUtcDateTime.AddHours(1), 
                CancellationToken.None);
            
            context.ContinueAsNew(null);
        }
    }
}
