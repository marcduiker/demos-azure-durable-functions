using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DurableFunctionsDemo.EternalOchestrationsAndEvents.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace DurableFunctionsDemo.EternalOchestrationsAndEvents.EventFunctions
{
    public static class UpdateCollection
    {
        [FunctionName("UpdateCollection")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", "delete", Route = "Collection/Update")]HttpRequestMessage req,
            [OrchestrationClient]DurableOrchestrationClient orchestrationClient,
            TraceWriter log)
        {
            var eventData = await req.Content.ReadAsAsync<UpdateCollectionEventData>();

            string eventName = req.Method == HttpMethod.Delete ? EventNames.RemoveName : EventNames.AddName;

            await orchestrationClient.RaiseEventAsync(
                eventData.OrchestrationInstanceId,
                eventName,
                eventData.Name);

             return req.CreateResponse(HttpStatusCode.OK);
        }
    }
}
