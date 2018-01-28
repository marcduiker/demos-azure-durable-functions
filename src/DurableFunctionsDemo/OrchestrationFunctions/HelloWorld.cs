using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace DurableFunctionsDemo.OrchestrationFunctions
{
    public static class BestMeetupFinder
    {
        [FunctionName("HelloWorld")]
        public static string Run(
            [OrchestrationTrigger]DurableOrchestrationContext context,
            TraceWriter log)
        {
            string name = context.GetInput<string>();
            log.Info($"HelloWorld function triggered with: {name}.");

            return $"Hello {name}";
        }
    }
}
