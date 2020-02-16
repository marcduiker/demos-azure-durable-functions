using System.Net.Http;
using System.Threading.Tasks;
using DurableFunctions.Demo.ScreenShots.Orchestrators;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.Demo.ScreenShots.Clients
{
    public class DemoQueueClient
    {
        [FunctionName(nameof(DemoQueueClient))]
        public async Task Run(
            [QueueTrigger("orchestrator-x-messages", Connection = "MessagingStorageConnection" )]
                string message,
            [DurableClient]IDurableClient orchestrationClient,
            ILogger log)
        {
            var instanceId = await orchestrationClient.StartNewAsync(
                nameof(DemoOrchestratorB),
                message);
        }
    }
}
