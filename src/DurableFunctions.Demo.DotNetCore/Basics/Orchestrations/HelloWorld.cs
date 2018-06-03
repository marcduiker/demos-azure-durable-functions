using System.Threading.Tasks;
using DurableFunctions.Demo.DotNetCore.Basics.Activities;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace DurableFunctions.Demo.DotNetCore.Basics.Orchestrations
{
    public static class HelloWorld
    {
        [FunctionName(nameof(HelloWorld))]
        public static async Task<string> Run(
            [OrchestrationTrigger]DurableOrchestrationContext context,
            TraceWriter log)
        {
            var result = await context.CallActivityAsync<string>(
                nameof(HelloWorldActivity),
                null);

            return result;
        }
    }
}
