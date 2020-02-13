using DurableFunctions.Demo.ScreenShots.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace DurableFunctions.Demo.ScreenShots.Activities
{
    public class ActivityFunctionQueueOutput
    {

        [FunctionName(nameof(ActivityFunctionQueueOutput))]
        [return: Queue("orchestrator-x-messages", Connection = "MessagingStorageConnection")]
        public string Run(
            [ActivityTrigger] string result)
        {
            return result;
        }
    }
}