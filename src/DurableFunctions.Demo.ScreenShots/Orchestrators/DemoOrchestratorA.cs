using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DurableFunctions.Demo.ScreenShots.Activities;
using DurableFunctions.Demo.ScreenShots.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.Demo.ScreenShots.Orchestrators
{
    public class DemoOrchestratorA
    {
        [FunctionName(nameof(DemoOrchestratorA))]
        public async Task<DemoOrchestratorAResult> Run(
            [OrchestrationTrigger]IDurableOrchestrationContext context,
            ILogger log)
        {
            var name = context.GetInput<string>();
        
            var result1 = await context.CallActivityAsync<Function1Result>(
                nameof(ActivityFunction1),
                name);
        
            var result2 = await context.CallActivityWithRetryAsync<Function2Result>(
                nameof(ActivityFunction2),
                GetDefaultRetryOptions(),
                result1);
        
            var orchestratorResult = new DemoOrchestratorAResult(result1, result2);
            return orchestratorResult;
        }

        private static RetryOptions GetDefaultRetryOptions()
        {
            return new RetryOptions(TimeSpan.FromSeconds(30), 3) { BackoffCoefficient = 2 };
        }
    }
}
