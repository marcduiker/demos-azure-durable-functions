using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DurableTask.Core;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.Demo.DotNetCore._02_Maintenance
{
    public class PurgeHistoryForMany
    {
        // https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-timer#ncrontab-expressions
        // https://cronexpressiondescriptor.azurewebsites.net
        // {second} {minute} {hour} {day} {month} {day-of-week}
        const string EveryDayAt730AM = "0 30 7 * * *";

        [FunctionName(nameof(PurgeHistoryForMany))]
        public async Task Run(
            [TimerTrigger(EveryDayAt730AM)]TimerInfo myTimer, 
            [DurableClient] IDurableClient client,
            ILogger log)
        {
            var instancesCreatedFromDate = DateTime.Today.Subtract(TimeSpan.FromDays(14));
            var instancesCreatedToDate = DateTime.Today.Subtract(TimeSpan.FromDays(7));
            var statussesToPurge = new List<OrchestrationStatus> {
                OrchestrationStatus.Completed,
                OrchestrationStatus.Terminated,
            };

            var purgeResult = await client.PurgeInstanceHistoryAsync(
                instancesCreatedFromDate,
                instancesCreatedToDate,
                statussesToPurge);

            log.LogInformation($"Purged {purgeResult.InstancesDeleted} instances.");
        }
    }
}
