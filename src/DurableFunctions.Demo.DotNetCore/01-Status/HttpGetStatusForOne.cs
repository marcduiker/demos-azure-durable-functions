using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

// ReSharper disable once CheckNamespace
namespace DurableFunctions.Demo.DotNetCore.Status
{
    public class HttpGetStatusForOne
    {
        /// <summary>
        /// This function retrives the status for one orchestration in the same Function App
        /// which matches the orchestrationName parameter.
        /// </summary>
        /// <param name="req">The HttpRequestMessage which can contain input data for the orchestration.</param>
        /// <param name="orchestrationClient">An instance of the DurableOrchestrationClient used to start a new orchestration.</param>
        /// <param name="id">Orchestration instance id to get the status for.</param>
        /// <param name="log">ILogger implementation.</param>
        /// <returns>A DurableOrchestrationStatus message.</returns>
        [FunctionName(nameof(HttpGetStatusForOne))]
        public async Task<DurableOrchestrationStatus> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "status/{id}")]HttpRequestMessage request,
            [OrchestrationClient]DurableOrchestrationClientBase orchestrationClient,
            string id,
            ILogger log)
        {
            DurableOrchestrationStatus status;

            var parameters = GetQueryStringParameters(request);
            if (parameters.hasParameters)
            {
                status = await orchestrationClient.GetStatusAsync(id, parameters.showHistory, parameters.showHostoryOutput);
            }    
            else
            {
                status = await orchestrationClient.GetStatusAsync(id);
            }

            return status;
        }

        private static (bool hasParameters, bool showHistory, bool showHostoryOutput) GetQueryStringParameters(HttpRequestMessage request)
        {

            bool hasParameters = request.RequestUri.ParseQueryString().HasKeys();
            bool showHistory = false;
            bool showHistoryOutput = false;
            if (hasParameters)
            {
                string showHistoryString = request.RequestUri.ParseQueryString().Get("showHistory");
                bool.TryParse(showHistoryString, out showHistory);
                string showHistoryOutputString = request.RequestUri.ParseQueryString().Get("showHistoryOutput");
                bool.TryParse(showHistoryOutputString, out showHistoryOutput);
            }

            return (hasParameters, showHistory, showHistoryOutput);
        }
    }
}
