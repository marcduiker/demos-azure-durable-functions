using System.Threading.Tasks;
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
            var meetupEvent = await orchestrationContext.CallActivityAsync<MeetupEvent>("GetMeetupEvent", meetupTravelInfoInput);
            var travelTimeInput = GetTravelTimeInput(meetupTravelInfoInput, meetupEvent);
            var result = await orchestrationContext.CallActivityAsync<JToken>("GetTravelTime", travelTimeInput);

            return result;
        }

        private static TravelTimeInput GetTravelTimeInput(MeetupTravelInfoInput input, MeetupEvent meetupEvent)
        {
            return new TravelTimeInput
            {
                EventStartUnixTimeSeconds = meetupEvent.UnixTimeMilliseconds / 1000,
                TravelMode = input.TravelMode,
                OriginAddress = input.OriginAddress,
                DestinationAddress = $"{meetupEvent.Venue.Name}, {meetupEvent.Venue.Address}, {meetupEvent.Venue.City}",
                TrafficModel = "pessimistic"
            };
        }
    }
}
