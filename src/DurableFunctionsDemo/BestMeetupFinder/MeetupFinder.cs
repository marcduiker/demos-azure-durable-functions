using System.Threading.Tasks;
using DurableFunctionsDemo.BestMeetupFinder.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json.Linq;

namespace DurableFunctionsDemo.BestMeetupFinder
{
    public static class MeetupFinder
    {
        [FunctionName("MeetupFinder")]
        public static async Task<JToken> Run(
            [OrchestrationTrigger]DurableOrchestrationContext orchestrationContext,
            TraceWriter log)
        {
            var findMeetupsInput = orchestrationContext.GetInput<FindMeetupsInput>();
            var result = await orchestrationContext.CallActivityAsync<JToken>("FindMeetups", findMeetupsInput);

            return result;
        }
    }
}
