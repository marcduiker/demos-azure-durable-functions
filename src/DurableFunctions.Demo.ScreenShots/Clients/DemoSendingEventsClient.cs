using System.Net.Http;
using System.Threading.Tasks;
using DurableFunctions.Demo.ScreenShots.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.Demo.ScreenShots.Clients
{
    public class DemoSendingEventsClient
    {
        [FunctionName(nameof(DemoSendingEventsClient))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "sendevent/{instanceId}/")]
            HttpRequestMessage req,
            string instanceId,
            [DurableClient] IDurableClient client,
            ILogger logger)
        {
            var approval = await req.Content.ReadAsAsync<Approval>();

            await client.RaiseEventAsync(instanceId, approval.EventName, approval.IsApproved);
            
            return new AcceptedResult();
        }
    }
}