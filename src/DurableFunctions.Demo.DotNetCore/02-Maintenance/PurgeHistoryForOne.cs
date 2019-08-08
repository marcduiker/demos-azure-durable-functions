using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace DurableFunctions.Demo.DotNetCore._02_Maintenance
{
    public class PurgeHistoryForOne
    {
        [FunctionName(nameof(PurgeHistoryForOne))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Admin, "DELETE", Route ="purge/{instanceId}")]HttpRequestMessage request,
            [OrchestrationClient] DurableOrchestrationClientBase client,
            string instanceId)
        {
            await client.PurgeInstanceHistoryAsync(instanceId);

            return new OkResult();
        }
    }
}
