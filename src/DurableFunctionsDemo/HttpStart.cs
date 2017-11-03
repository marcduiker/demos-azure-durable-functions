using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace DurableFunctionsDemo
{
    public static class HttpStart
    {
        [FunctionName("HttpStart")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "orchestration/{functionName}")]HttpRequestMessage req, 
            [OrchestrationClient]DurableOrchestrationClient orchestrationClient,
            string functionName,
            TraceWriter log)
        {
            log.Info("HttpStart triggered.");

            dynamic eventData = await req.Content.ReadAsAsync<object>();
            string instanceId = await orchestrationClient.StartNewAsync(functionName, eventData);

            log.Info($"Started orchestration with ID = '{instanceId}'.");

            return orchestrationClient.CreateCheckStatusResponse(req, instanceId);
        }
    }
}
