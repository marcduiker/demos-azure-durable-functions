using System;
using System.Linq;
using System.Threading.Tasks;
using DurableFunctions.Demo.DotNetCore.Activities;
using DurableFunctions.Demo.DotNetCore.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.Demo.DotNetCore._40_Events.Orchestrations
{
    public class MultipleApprovalOrchestrator
    {
        [FunctionName(nameof(MultipleApprovalOrchestrator))]
        public async Task Run(
            [OrchestrationTrigger] IDurableOrchestrationContext context,
            ILogger logger)
        {
            var message = context.GetInput<Message>();
            var timeout = TimeSpan.FromHours(24);
            var defaultApproval = ApprovalBuilder.BuildDefault();

            var approval1 = context.WaitForExternalEvent(
                EventNames.MultipleApproveEvents1,
                timeout,
                defaultApproval);

            var approval2 = context.WaitForExternalEvent(
                EventNames.MultipleApproveEvents2,
                timeout,
                defaultApproval);

            var approvals = await Task.WhenAll(approval1, approval2);

            var putOnQueueActivity = approvals.All(a => a.IsApproved)
                ? nameof(PutOnApprovedMessagesQueueActivity)
                : nameof(PutOnDeniedMessagesQueueActivity);

            await context.CallActivityAsync<Message>(
                putOnQueueActivity,
                message);

            context.SetCustomStatus(new { QueueActivityName = putOnQueueActivity} );
        }
    }
}