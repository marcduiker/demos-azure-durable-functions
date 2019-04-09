using System;
using System.Threading.Tasks;
using DurableFunctions.Demo.DotNetCore.Basics.Activities;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

// ReSharper disable once CheckNamespace
namespace DurableFunctions.Demo.DotNetCore.Basics.Orchestrations
{
    public class FailingOrchestration
    {
        [FunctionName(nameof(FailingOrchestration))]
        public async Task<string> Run(
            [OrchestrationTrigger]DurableOrchestrationContextBase context,
            ILogger log)
        {
            var result = await context.CallActivityWithRetryAsync<string>(
                nameof(FailingActivity),
                new RetryOptions(TimeSpan.FromSeconds(5), 3),
                string.Empty);

            return result;
        }
    }
}
