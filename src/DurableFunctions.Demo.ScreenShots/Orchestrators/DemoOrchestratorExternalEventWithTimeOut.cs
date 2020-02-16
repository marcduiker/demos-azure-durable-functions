using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.Demo.ScreenShots.Orchestrators
{
    public class DemoOrchestratorExternalEventWithTimeOut
    {
        [FunctionName(nameof(DemoOrchestratorExternalEventWithTimeOut))]
        public async Task Run(
            [OrchestrationTrigger] IDurableOrchestrationContext context,
            ILogger logger)
        {
            var timeout = TimeSpan.FromHours(48);

            var approval1 = context.WaitForExternalEvent<bool>("ApprovalEvent1", timeout, false);
            var approval2 = context.WaitForExternalEvent<bool>("ApprovalEvent2", timeout, false);
            var approval3 = context.WaitForExternalEvent<bool>("ApprovalEvent3", timeout, false);

            var approvals = await Task.WhenAll(approval1, approval2, approval3);
            var approvalResult = approvals.All(a => a);
            if (approvalResult)
            {
                // All are approved, happy flow
            }
            else
            {
                // One or more are not approved, unhappy flow
            }
        }
    }
}