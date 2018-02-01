using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace DurableFunctionsDemo.Basics
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
