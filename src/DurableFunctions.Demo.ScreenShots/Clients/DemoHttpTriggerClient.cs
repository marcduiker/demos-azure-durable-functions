using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.Demo.ScreenShots.Clients
{
    public class DemoHttpTriggerClient
    {
        [FunctionName(nameof(DemoHttpTriggerClient))]
        public async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "start/{orchestratorName}/")]
                HttpRequestMessage req,
            [DurableClient]IDurableClient orchestrationClient,
            string orchestratorName,
            ILogger log)
        {
            var orchestratorInput = await req.Content.ReadAsAsync<object>();

            var instanceId = await orchestrationClient.StartNewAsync(
                orchestratorName,
                orchestratorInput);

            return orchestrationClient.CreateCheckStatusResponse(req, instanceId);
        }
    }
}
