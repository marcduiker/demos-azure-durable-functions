using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace DurableFunctions.Demo.DotNetCore._02_Maintenance
{
    public class TerminateInstance
    {
        [FunctionName(nameof(TerminateInstance))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Admin, "POST", Route ="terminate/{instanceId}")]HttpRequestMessage request,
            [DurableClient] IDurableClient client,
            string instanceId)
        {
            var reason = await request.Content.ReadAsAsync<string>();
            await client.TerminateAsync(instanceId, reason);

            return new AcceptedResult();
        }
    }
}
