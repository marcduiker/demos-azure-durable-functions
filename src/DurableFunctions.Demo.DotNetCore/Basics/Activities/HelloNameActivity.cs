using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.Demo.DotNetCore.Basics.Activities
{
    public static class HelloNameActivity
    {
        [FunctionName(nameof(HelloNameActivity))]
        public static string Run(
            [ActivityTrigger] DurableActivityContext context,
            ILogger logger)
        {
            var name = context.GetInput<string>();

            logger.LogInformation($"Name: {name}");

            return $"Hello {name}!";
        }
    }
}
