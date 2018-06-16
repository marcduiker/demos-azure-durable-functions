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
            return "Hello World";
        }
    }
}
