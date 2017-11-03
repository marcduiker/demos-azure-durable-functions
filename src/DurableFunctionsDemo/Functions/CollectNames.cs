using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace DurableFunctionsDemo.Functions
{
    public static class CollectNames
    {
        [FunctionName("CollectNames")]
        public static async Task<List<string>> Run(
            [OrchestrationTrigger]DurableOrchestrationContext context,
            TraceWriter log)
        {
            var names = context.GetInput<List<string>>();
            if (names == null)
            {
                names = new List<string>();
            }

            var nameTask = context.WaitForExternalEvent<string>("addname");
            var isCompletedTask = context.WaitForExternalEvent<bool>("iscompleted");

            var resultingEvent = await Task.WhenAny(nameTask, isCompletedTask);

            if (resultingEvent == nameTask)
            {
                string name = nameTask.Result;
                names.Add(name);
                log.Info($"Added {name} to the list.");
            }

            if (resultingEvent == isCompletedTask &&
                isCompletedTask.Result)
            {
                log.Info("Completed adding names.");
            }
            else
            {
                context.ContinueAsNew(names);
            }


            return names;
        }
    }
}
