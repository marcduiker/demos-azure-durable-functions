using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace DurableFunctions.Demo.DotNetCore.Basics
{
    public static class HelloWorld
    {
        [FunctionName("HelloWorld")]
        public static string Run(
            [OrchestrationTrigger]DurableOrchestrationContext context,
            TraceWriter log)
        {
            string name = context.GetInput<string>();

            return $"Hello {name}";
        }
    }
}
