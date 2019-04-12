using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

// ReSharper disable once CheckNamespace
namespace DurableFunctions.Demo.DotNetCore.Starters
{
    public class HttpStartAndWait
    {
        /// <summary>
        /// This function starts a new Orchestrator in the same Function App
        /// which matches the functionName parameter. If the Orchestrator completes
        /// within the specified timeout the actual Orchestrator output is returned in the HttpResponseMessage.
        /// If the Orchestrator takes longer to complete then the CheckStatusResponse message is returned instead.
        /// </summary>
        /// <param name="request">The HttpRequestMessage which can contain input data for the Orchestrator.</param>
        /// <param name="orchestratorClient">An instance of the DurableOrchestrationClient used to start a new Orchestrator.</param>
        /// <param name="orchestratorName">The name of the Orchestrator function to start.</param>
        /// <param name="log">ILogger implementation.</param>
        /// <returns>An HttpResponseMessage containing the id and status of the Orchestrator instance.</returns>
        [FunctionName(nameof(HttpStartAndWait))]
        public async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "startandwait/{orchestratorName}")]HttpRequestMessage request, 
            [OrchestrationClient]DurableOrchestrationClientBase orchestratorClient,
            string orchestratorName,
            ILogger log)
        { 
            dynamic orchestratorInput = await request.Content.ReadAsAsync<object>();
            string instanceId = await orchestratorClient.StartNewAsync(
                orchestratorName, 
                orchestratorInput);

            log.LogInformation($"Started Orchestrator with ID = '{instanceId}'...");

            var timeoutTime = GetTimeSpan(request, TimeoutQueryStringKey);
            var retryIntervalTime = GetTimeSpan(request, RetryIntervalQueryStringKey);

            HttpResponseMessage responseMessage = null;

            if (timeoutTime == TimeSpan.Zero && retryIntervalTime == TimeSpan.Zero)
            {
                // Wait using the default values in the Durable Functions extension (10 sec timeout, 1 sec interval):
                responseMessage = await orchestratorClient.WaitForCompletionOrCreateCheckStatusResponseAsync(
                    request,
                    instanceId);
            }

            if (timeoutTime != TimeSpan.Zero && retryIntervalTime == TimeSpan.Zero)
            {
                // Wait until the specified timeoutTime:
                responseMessage = await orchestratorClient.WaitForCompletionOrCreateCheckStatusResponseAsync(
                    request,
                    instanceId,
                    timeoutTime);
            }

            if (timeoutTime != TimeSpan.Zero && retryIntervalTime != TimeSpan.Zero)
            {
                // Wait until the specified timeoutTime and check every retryIntervalTime:
                responseMessage = await orchestratorClient.WaitForCompletionOrCreateCheckStatusResponseAsync(
                    request,
                    instanceId,
                    timeoutTime,
                    retryIntervalTime);
            }

            return responseMessage;
        }

        private static TimeSpan GetTimeSpan(HttpRequestMessage request, string queryParameterName)
        {
            string queryParameterStringValue = request.RequestUri.ParseQueryString()[queryParameterName];
            if (string.IsNullOrEmpty(queryParameterStringValue))
            {
                return TimeSpan.Zero;
            }

            return TimeSpan.FromSeconds(double.Parse(queryParameterStringValue));
        }

        private const string TimeoutQueryStringKey = "timeout";
        private const string RetryIntervalQueryStringKey = "interval";
    }
}
