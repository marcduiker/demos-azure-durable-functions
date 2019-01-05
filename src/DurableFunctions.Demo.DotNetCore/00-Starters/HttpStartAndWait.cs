using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

// ReSharper disable once CheckNamespace
namespace DurableFunctions.Demo.DotNetCore.Starters
{
    public static class HttpStartAndWait
    {
        /// <summary>
        /// This function starts a new orchestration in the same Function App
        /// which matches the functionName parameter. If the orchestration completes
        /// within the specified timeout the actual orchestration output is returned in the HttpResponseMessage.
        /// If the orchestration takes longer to complete then the CheckStatusResponse message is returned instead.
        /// </summary>
        /// <param name="request">The HttpRequestMessage which can contain input data for the orchestration.</param>
        /// <param name="orchestrationClient">An instance of the DurableOrchestrationClient used to start a new orchestration.</param>
        /// <param name="functionName">The name of the orchestration function to start.</param>
        /// <param name="log">ILogger implementation.</param>
        /// <returns>An HttpResponseMessage containing the id and status of the orchestration instance.</returns>
        [FunctionName(nameof(HttpStartAndWait))]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "startandwait/{functionName}")]HttpRequestMessage request, 
            [OrchestrationClient]DurableOrchestrationClientBase orchestrationClient,
            string functionName,
            ILogger log)
        { 
            dynamic functionData = await request.Content.ReadAsAsync<object>();
            string instanceId = await orchestrationClient.StartNewAsync(functionName, functionData);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'...");

            var timeoutTime = GetTimeSpan(request, TimeoutQueryStringKey);
            var retryIntervalTime = GetTimeSpan(request, RetryIntervalQueryStringKey);

            HttpResponseMessage responseMessage = null;

            if (timeoutTime != TimeSpan.Zero && retryIntervalTime != TimeSpan.Zero)
            {
                // Wait until the specified timeoutTime and check every retryIntervalTime:
                responseMessage = await orchestrationClient.WaitForCompletionOrCreateCheckStatusResponseAsync(
                    request,
                    instanceId,
                    timeoutTime,
                    retryIntervalTime);
            }

            if (timeoutTime != TimeSpan.Zero && retryIntervalTime == TimeSpan.Zero)
            {
                // Wait until the specified timeoutTime:
                responseMessage = await orchestrationClient.WaitForCompletionOrCreateCheckStatusResponseAsync(
                    request,
                    instanceId,
                    timeoutTime);
            }

            if (timeoutTime == TimeSpan.Zero && retryIntervalTime == TimeSpan.Zero)
            {
                // Wait using the default values in the Durable Functions extension (10 sec timeout, 1 sec interval):
                responseMessage = await orchestrationClient.WaitForCompletionOrCreateCheckStatusResponseAsync(
                    request,
                    instanceId);
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
