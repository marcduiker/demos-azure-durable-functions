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
    public class DemoOrchestratorB
    {
        [FunctionName(nameof(DemoOrchestratorB))]
        public async Task Run(
            [OrchestrationTrigger]IDurableOrchestrationContext context,
            ILogger log)
        {
        }
    }
}
