using System.Net.Http;
using System.Threading.Tasks;
using DurableFunctions.Demo.ScreenShots.Orchestrators;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.Demo.ScreenShots.Clients
{
    public class DemoMultipleOrchestrationsHttpTriggerClient
    {
        [FunctionName(nameof(DemoMultipleOrchestrationsHttpTriggerClient))]
        public async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "multiple")]
                HttpRequestMessage req,
            [DurableClient]IDurableClient orchestrationClient,
                ILogger log)
        {
            var orchestratorInput = await req.Content.ReadAsAsync<object>();

            var instanceIdA = await orchestrationClient.StartNewAsync(
                nameof(DemoOrchestratorA),
                orchestratorInput);
            var instanceIdB = await orchestrationClient.StartNewAsync(
                nameof(DemoOrchestratorB),
                orchestratorInput);

            return orchestrationClient.CreateCheckStatusResponse(req, instanceIdB);
        }
    }
}
