using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

// ReSharper disable once CheckNamespace
namespace DurableFunctions.Demo.DotNetCore.Status
{
    public class HttpGetStatusForOne
    {
        /// <summary>
        /// This function retrives the status for one Orchestrator in the same Function App
        /// which matches the OrchestratorName parameter.
        /// </summary>
        /// <param name="req">The HttpRequestMessage which can contain input data for the Orchestrator.</param>
        /// <param name="orchestratorClient">An instance of the DurableOrchestrationClient used to start a new Orchestrator.</param>
        /// <param name="id">Orchestrator instance id to get the status for.</param>
        /// <param name="log">ILogger implementation.</param>
        /// <returns>A DurableOrchestrationStatus message.</returns>
        [FunctionName(nameof(HttpGetStatusForOne))]
        public async Task<DurableOrchestrationStatus> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "status/{id}")]HttpRequestMessage request,
            [DurableClient]IDurableClient orchestratorClient,
            string id,
            ILogger log)
        {
            DurableOrchestrationStatus status;

            var parameters = GetQueryStringParameters(request);
            if (parameters.hasParameters)
            {
                status = await orchestratorClient.GetStatusAsync(
                    id, 
                    parameters.showHistory, 
                    parameters.showHistoryOutput, 
                    parameters.showInput);
            }    
            else
            {
                status = await orchestratorClient.GetStatusAsync(id);
            }

            return status;
        }

        private static (bool hasParameters, bool showHistory, bool showHistoryOutput, bool showInput) GetQueryStringParameters(HttpRequestMessage request)
        {

            bool hasParameters = request.RequestUri.ParseQueryString().HasKeys();
            bool showHistory = false;
            bool showHistoryOutput = false;
            bool showInput = false;
            if (hasParameters)
            {
                string showHistoryString = request.RequestUri.ParseQueryString().Get("showHistory");
                bool.TryParse(showHistoryString, out showHistory);
                string showHistoryOutputString = request.RequestUri.ParseQueryString().Get("showHistoryOutput");
                bool.TryParse(showHistoryOutputString, out showHistoryOutput);
                string showInputString = request.RequestUri.ParseQueryString().Get("showInput");
                bool.TryParse(showInputString, out showInput);
            }

            return (hasParameters, showHistory, showHistoryOutput, showInput);
        }
    }
}
