using System;
using System.Threading.Tasks;
using DurableFunctions.Demo.DotNetCore.Basics.Activities;
using DurableFunctions.Demo.DotNetCore.Retry.Activities;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

// ReSharper disable once CheckNamespace
namespace DurableFunctions.Demo.DotNetCore.Retry.Orchestrators
{
    public class FailingOrchestrator
    {
        [FunctionName(nameof(FailingOrchestrator))]
        public async Task<string> Run(
            [OrchestrationTrigger]IDurableOrchestrationContext context,
            ILogger log)
        {
            var result = await context.CallActivityWithRetryAsync<string>(
                nameof(FailingActivity),
                new RetryOptions(
                    firstRetryInterval: TimeSpan.FromSeconds(5),
                    maxNumberOfAttempts: 3),
                string.Empty);

            return result;
        }
    }
}
