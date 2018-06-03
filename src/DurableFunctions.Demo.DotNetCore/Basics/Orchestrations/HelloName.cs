using System.Threading.Tasks;
using DurableFunctions.Demo.DotNetCore.Basics.Activities;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace DurableFunctions.Demo.DotNetCore.Basics.Orchestrations
{
    public static class HelloName
    {
        [FunctionName(nameof(HelloName))]
        public static async Task<string> Run(
            [OrchestrationTrigger]DurableOrchestrationContext context,
            TraceWriter log)
        {
            var name = context.GetInput<string>();

            var result = await context.CallActivityAsync<string>(
                nameof(HelloNameActivity),
                name);

            return result;
        }
    }
}
