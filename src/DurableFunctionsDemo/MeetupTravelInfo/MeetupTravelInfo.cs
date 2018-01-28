using System.Threading.Tasks;
using DurableFunctionsDemo.BestMeetupFinder.Models;
using DurableFunctionsDemo.MeetupTravelInfo.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json.Linq;

namespace DurableFunctionsDemo.MeetupTravelInfo
{
    public static class MeetupTravelInfo
    {
        [FunctionName("MeetupTravelInfo")]
        public static async Task<JToken> Run(
            [OrchestrationTrigger]DurableOrchestrationContext orchestrationContext,
            TraceWriter log)
        {
            var meetupTravelInfoInput = orchestrationContext.GetInput<MeetupTravelInfoInput>();
            var result = await orchestrationContext.CallActivityAsync<JToken>("GetMeetupEvent", meetupTravelInfoInput);

            return result;
        }
    }
}
