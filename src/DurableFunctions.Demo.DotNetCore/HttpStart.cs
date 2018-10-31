using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.Demo.DotNetCore
{
    public static class HttpStart
    {
        [FunctionName(nameof(HttpStart))]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "orchestration/{functionName}")]HttpRequestMessage req, 
            [OrchestrationClient]DurableOrchestrationClientBase orchestrationClient,
            string functionName,
            ILogger log)
        {
            dynamic functionData = await req.Content.ReadAsAsync<object>();
            string instanceId = await orchestrationClient.StartNewAsync(
                functionName, 
                functionData);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'...");

            return orchestrationClient.CreateCheckStatusResponse(req, instanceId);
        }
    }
}
