using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

// ReSharper disable once CheckNamespace
namespace DurableFunctions.Demo.DotNetCore.Basics.Activities
{
    public class HelloWorldActivity
    {
        [FunctionName(nameof(HelloWorldActivity))]
        public string Run(
            // IDurableActivityContext -> IIDurableActivityContext
            [ActivityTrigger] IDurableActivityContext activityContext,
            ILogger logger)
        {
            logger.Log(
                LogLevel.Information, 
                $"Triggered {nameof(HelloWorldActivity)} - instance {activityContext.InstanceId}");

            return "Hello World!";
        }
    }
}
