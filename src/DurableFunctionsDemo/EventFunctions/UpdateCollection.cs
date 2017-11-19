using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DurableFunctionsDemo.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace DurableFunctionsDemo.EventFunctions
{
    public static class UpdateCollection
    {
        [FunctionName("UpdateCollection")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "UpdateCollection")]HttpRequestMessage req,
            [OrchestrationClient]DurableOrchestrationClient orchestrationClient,
            TraceWriter log)
        {
            var eventData = await req.Content.ReadAsAsync<UpdateCollectionEventData>();

            await orchestrationClient.RaiseEventAsync(
                eventData.OrchestrationInstanceId,
                eventData.EventName,
                eventData.Name);

             return req.CreateResponse(HttpStatusCode.OK);
        }
    }
}
