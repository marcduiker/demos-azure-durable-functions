using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace DurableFunctions.Demo.DotNetCore.Basics.Activities
{
    public static class HelloNameActivity
    {
        [FunctionName(nameof(HelloNameActivity))]
        public static string Run(
            [ActivityTrigger] DurableActivityContext context,
            TraceWriter logger)
        {
            var name = context.GetInput<string>();

            logger.Info($"Name: {name}");

            return $"Hello {name}!";
        }
    }
}
