using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

// ReSharper disable once CheckNamespace
namespace DurableFunctions.Demo.DotNetCore.Starters
{
    public class HttpStart
    {

        /// <summary>
        /// This function starts a new Orchestrator in the same Function App
        /// which matches the OrchestratorName parameter.
        /// </summary>
        /// <param name="req">The HttpRequestMessage which can contain input data for the Orchestrator.</param>
        /// <param name="orchestratorClient">An instance of the DurableOrchestrationClient used to start a new Orchestrator.</param>
        /// <param name="orchestratorName">The name of the Orchestrator function to start.</param>
        /// <param name="id">Optional id for the Orchestrator function instance.</param>
        /// <param name="log">ILogger implementation.</param>
        /// <returns>An HttpResponseMessage containing the id and status of the Orchestrator instance.</returns>
        [FunctionName(nameof(HttpStart))]
        public async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "start/{orchestratorName}/{id?}")]HttpRequestMessage req,
            [DurableClient]IDurableClient orchestratorClient,
            string orchestratorName,
            string id,
            ILogger log)
        {
            var orchestratorInput = await req.Content.ReadAsAsync<object>();

            string instanceId = id;
            if (string.IsNullOrEmpty(instanceId))
            {
                // Start a new Orchestrator and let Durable Functions generate the instance id.
                instanceId = await orchestratorClient.StartNewAsync(
                    orchestratorName,
                    orchestratorInput);
            }
            else
            {
                // Start a new Orchestrator and use your own instance id.
                instanceId = await orchestratorClient.StartNewAsync(
                    orchestratorName,
                    instanceId,
                    orchestratorInput);
            }

            log.LogInformation($"Started Orchestrator with ID = '{instanceId}'...");

            return orchestratorClient.CreateCheckStatusResponse(req, instanceId);
        }
    }
}
