using System;
using System.Net.Http;
using System.Threading.Tasks;
using DurableFunctions.Demo.ScreenShots.Activities;
using DurableFunctions.Demo.ScreenShots.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.Demo.ScreenShots.Orchestrators
{
    public class DemoOrchestratorsBetweenFunctionApps
    {
        [FunctionName(nameof(RunOrchestratorWithHttpCall))]
        public async Task RunOrchestratorWithHttpCall(
            [OrchestrationTrigger]IDurableOrchestrationContext context,
            ILogger log)
        {
            // Call Activity Functions as usual.
            // ...
            var activityResult = "ResultFromTheActivities";
            
            // Finally call an orchestration in another Function App:
            var uri = new Uri("https://anotherfunctionapp/start/nextorchestrator");
            await context.CallHttpAsync(HttpMethod.Post, uri, activityResult);
        }
        
        [FunctionName(nameof(RunOrchestratorWithActivityQueueBinding))]
        public async Task RunOrchestratorWithActivityQueueBinding(
            [OrchestrationTrigger]IDurableOrchestrationContext context,
            ILogger log)
        {
            // Call Activity Functions as usual.
            // ...
            var activityResult = "ResultFromTheActivities";
            
            // Finally put a message on a queue which
            // can be picked up by another Function App:
            await context.CallActivityAsync<string>(
                nameof(ActivityFunctionQueueOutput),
                activityResult);
        }
    }
}
