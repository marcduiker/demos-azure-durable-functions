using System.Threading.Tasks;
using DurableFunctions.Demo.DotNetCore.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.Demo.DotNetCore.Activities
{
    public class PutMessageOnQueueActivity
    {
        [FunctionName(nameof(PutMessageOnQueueActivity))]
        [return: Queue("QueueName", Connection = "ConnectionName")]
        public Message Run(
            [ActivityTrigger] Message message,
            ILogger logger)
        {
            return message;
        }
    }
}