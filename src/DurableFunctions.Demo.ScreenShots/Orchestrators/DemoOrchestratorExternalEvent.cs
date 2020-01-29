using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.Demo.ScreenShots.Orchestrators
{
    public class DemoOrchestratorExternalEvent
    {
        [FunctionName(nameof(DemoOrchestratorExternalEvent))]
        public async Task Run(
            [OrchestrationTrigger] IDurableOrchestrationContext context,
            ILogger logger)
        {
            var approval1 = context.WaitForExternalEvent<bool>("ApprovalEvent1");
            var approval2 = context.WaitForExternalEvent<bool>("ApprovalEvent2");
            var approval3 = context.WaitForExternalEvent<bool>("ApprovalEvent3");
            
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