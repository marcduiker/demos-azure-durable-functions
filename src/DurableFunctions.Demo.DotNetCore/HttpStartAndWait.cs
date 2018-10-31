using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.Demo.DotNetCore
{
    public static class HttpStartAndWait
    {
        private const string Timeout = "timeout";
        private const string RetryInterval = "interval";
        private const int DefaultTime = 5;

        [FunctionName(nameof(HttpStartAndWait))]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "orchestration/{functionName}/wait")]HttpRequestMessage request, 
            [OrchestrationClient]DurableOrchestrationClientBase orchestrationClient,
            string functionName,
            ILogger log)
        { 
            dynamic functionData = await request.Content.ReadAsAsync<object>();
            string instanceId = await orchestrationClient.StartNewAsync(functionName, functionData);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'...");

            var timeoutTime = GetTimeSpan(request, Timeout);
            var retryIntervalTime = GetTimeSpan(request, RetryInterval);

            return await orchestrationClient.WaitForCompletionOrCreateCheckStatusResponseAsync(
                request, 
                instanceId,
                timeoutTime,
                retryIntervalTime);
        }

        private static TimeSpan GetTimeSpan(HttpRequestMessage request, string queryParameterName)
        {
            string queryParameterStringValue = request.RequestUri.ParseQueryString()[queryParameterName];
            if (string.IsNullOrEmpty(queryParameterStringValue))
            {
                return TimeSpan.FromSeconds(DefaultTime);
            }

            return TimeSpan.FromSeconds(double.Parse(queryParameterStringValue));
        }
    }
}
