using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

// ReSharper disable once CheckNamespace
namespace DurableFunctions.Demo.DotNetCore.Status
{
    public class HttpGetStatusForMany
    {
        /// <summary>
        /// This function retrives the status for several orchestrations in the same Function App.
        /// </summary>
        /// <param name="req">The HttpRequestMessage which contains the GetStatusRequest data.</param>
        /// <param name="orchestrationClient">An instance of the DurableOrchestrationClient used to start a new orchestration.</param>
        /// <param name="log">ILogger implementation.</param>
        /// <returns>A collection of DurableOrchestrationStatus messages.</returns>
        [FunctionName(nameof(HttpGetStatusForMany))]
        public async Task<IList<DurableOrchestrationStatus>> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "status")]HttpRequestMessage req,
            [OrchestrationClient]DurableOrchestrationClientBase orchestrationClient,
            ILogger log)
        {
            var getStatusRequest = await req.Content.ReadAsAsync<GetStatusRequest>();

            IList<DurableOrchestrationStatus> results = new List<DurableOrchestrationStatus>();

            if (getStatusRequest.CreatedFrom.HasValue && getStatusRequest.StatussesToMatch.Any())
            {
                results = await orchestrationClient.GetStatusAsync(
                    getStatusRequest.CreatedFrom.Value,
                    getStatusRequest.CreatedTo,
                    getStatusRequest.StatussesToMatch);
            }
            else
            {
                results = await orchestrationClient.GetStatusAsync();
            }

            return results;
        }
    }
}
