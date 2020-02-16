using System.Net.Http;
using System.Threading.Tasks;
using DurableFunctions.Demo.DotNetCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.Demo.DotNetCore.Clients
{
    public class RaiseApprovalEventClient
    {
        [FunctionName(nameof(RaiseApprovalEventClient))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "approval/{eventName}/{instanceId}")]
            HttpRequestMessage req,
            [DurableClient] IDurableClient client,
            string eventName,
            string instanceId,
            ILogger log)
        {
            var approval = await req.Content.ReadAsAsync<Approval>();

            await client.RaiseEventAsync(instanceId, eventName, approval);
            
            return new AcceptedResult();
        }
    }
}