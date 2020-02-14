using System;
using System.Threading.Tasks;
using DurableFunctions.Demo.DotNetCore.Activities;
using DurableFunctions.Demo.DotNetCore.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.Demo.DotNetCore._40_Events.Orchestrations
{
    public class SingleApprovalWithTimeOutOrchestrator
    {
        [FunctionName(nameof(SingleApprovalWithTimeOutOrchestrator))]
        public async Task Run(
            [OrchestrationTrigger] IDurableOrchestrationContext context,
            ILogger logger)
        {
            var message = context.GetInput<Message>();

            var approval = await context.WaitForExternalEvent(
                EventNames.SingleApproveEvent,
                TimeSpan.FromSeconds(10),
                ApprovalBuilder.BuildDefault());

             var putOnQueueActivity = approval.IsApproved ? 
                nameof(PutOnApprovedMessagesQueueActivity) : 
                nameof(PutOnDeniedMessagesQueueActivity);

             await context.CallActivityAsync<Message>(
                putOnQueueActivity,
                message);
             
             context.SetCustomStatus(new { QueueActivityName = putOnQueueActivity} );
        }
    }
}