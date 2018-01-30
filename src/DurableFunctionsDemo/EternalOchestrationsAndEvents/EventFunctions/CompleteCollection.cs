using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DurableFunctionsDemo.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace DurableFunctionsDemo.EternalOchestrationsAndEvents.EventFunctions
{
    public static class CompleteCollection
    {
        [FunctionName("CompleteCollection")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "Collection/Complete")]HttpRequestMessage req,
            [OrchestrationClient]DurableOrchestrationClient orchestrationClient,
            TraceWriter log)
        {
            var eventData = await req.Content.ReadAsAsync<CompleteCollectionEventData>();

            await orchestrationClient.RaiseEventAsync(
                eventData.OrchestrationInstanceId,
                EventNames.IsCompleted,
                eventData.IsCompleted);

             return req.CreateResponse(HttpStatusCode.OK);
        }
    }
}
