using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

// ReSharper disable once CheckNamespace
namespace DurableFunctions.Demo.DotNetCore.Basics.Activities
{
    public static class HelloWorldActivity
    {
        [FunctionName(nameof(HelloWorldActivity))]
        public static string Run(
            [ActivityTrigger] DurableActivityContext activityContext,
            ILogger logger)
        {
            logger.Log(
                LogLevel.Information, 
                $"Triggered {nameof(HelloWorldActivity)} - instance {activityContext.InstanceId}");

            return "Hello World!";
        }
    }
}
