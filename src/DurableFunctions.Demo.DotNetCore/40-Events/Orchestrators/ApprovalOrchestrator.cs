using System.Threading.Tasks;
using DurableFunctions.Demo.DotNetCore.Activities;
using DurableFunctions.Demo.DotNetCore.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.Demo.DotNetCore._40_Events.Orchestrations
{
    public class ApprovalOrchestrator
    {
        [FunctionName(nameof(ApprovalOrchestrator))]
        public async Task Run(
            [OrchestrationTrigger] IDurableOrchestrationContext context,
            ILogger logger)
        {
            var message = context.GetInput<Message>();
            
            var approval = await context.WaitForExternalEvent<Approval>(
                EventNames.ApproveEvent);
            
            if (approval.Result)
            {
                await context.CallActivityAsync<Message>(
                    nameof(PutMessageOnQueueActivity),
                    message);
            }

        }
    }
}