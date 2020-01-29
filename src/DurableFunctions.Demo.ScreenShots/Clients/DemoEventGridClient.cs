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
    public class DemoEventGridClient
    {
        [FunctionName(nameof(DemoEventGridClient))]
        public async Task Run(
            [EventGrid(TopicEndpointUri = "", TopicKeySetting = "" )]EventGridEvent eventGridEvent,
            [DurableClient]IDurableClient orchestrationClient,
            string orchestratorName,
            ILogger log)
        {
            var orchestratorInput = eventGridEvent.Data;

            var instanceId = await orchestrationClient.StartNewAsync(
                nameof(DemoOrchestratorB),
                orchestratorInput);
        }
    }
}
