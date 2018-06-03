using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace DurableFunctions.Demo.DotNetCore.Basics.Activities
{
    public static class HelloWorldActivity
    {
        [FunctionName(nameof(HelloWorldActivity))]
        public static string Run(
            [ActivityTrigger] DurableActivityContext activityContext,
            TraceWriter logger)
        {
            return "Hello World";
        }
    }
}
