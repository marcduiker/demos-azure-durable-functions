using System.Collections.Generic;
using System.Threading.Tasks;
using DurableFunctions.Demo.DotNetCore.EternalOchestrationsAndEvents.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.Demo.DotNetCore.EternalOchestrationsAndEvents.Orchestrations
{
    public static class CollectNames
    {
        [FunctionName(nameof(CollectNames))]
        public static async Task<List<string>> Run(
            [OrchestrationTrigger]DurableOrchestrationContext context,
            ILogger log)
        {
            var nameList = context.GetInput<List<string>>() ?? new List<string>();

            var addNameTask = context.WaitForExternalEvent<string>(EventNames.AddName);
            var removeNameTask = context.WaitForExternalEvent<string>(EventNames.RemoveName);
            var isCompletedTask = context.WaitForExternalEvent<bool>(EventNames.IsCompleted);

            var resultingEvent = await Task.WhenAny(addNameTask, removeNameTask, isCompletedTask);

            if (resultingEvent == addNameTask)
            {
                nameList.Add(addNameTask.Result);
                log.LogInformation($"Added {addNameTask.Result} to the list.");
            }
            else if (resultingEvent == removeNameTask)
            {
                nameList.Remove(removeNameTask.Result);
                log.LogInformation($"Removed {removeNameTask.Result} from the list.");
            }

            if (resultingEvent == isCompletedTask &&
                isCompletedTask.Result)
            {
                log.LogInformation("Completed updating the list.");
            }
            else
            {
                context.ContinueAsNew(nameList);
            }

            return nameList;
        }
    }
}
