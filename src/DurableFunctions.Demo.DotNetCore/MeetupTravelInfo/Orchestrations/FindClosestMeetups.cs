using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DurableFunctions.Demo.DotNetCore.MeetupTravelInfo.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.Demo.DotNetCore.MeetupTravelInfo.Orchestrations
{
    public static class FindClosestMeetups
    {
        [FunctionName(nameof(FindClosestMeetups))]
        public static async Task<TravelInfo[]> Run(
            [OrchestrationTrigger]DurableOrchestrationContext orchestrationContext,
            ILogger log)
        {
            var input = orchestrationContext.GetInput<FindClosestMeetupsInput>();
            var meetupEvents = await orchestrationContext.CallActivityAsync<MeetupEvent[]>("GetUpcomingEventsByText", input);

            var tasks = new List<Task<TravelInfo>>();
            foreach (var meetupEvent in meetupEvents)
            {
                var travelTimeInput = GetTravelTimeInput(input, meetupEvent);
                tasks.Add(
                    orchestrationContext.CallActivityAsync<TravelInfo>(
                        "GetTravelTime", 
                        travelTimeInput)
                    );
            }

            await Task.WhenAll(tasks);

            return tasks.Select(task => task.Result)
                .OrderBy(travelInfo => travelInfo.DurationSeconds)
                .ToArray();
        }

        private static TravelTimeInput GetTravelTimeInput(FindClosestMeetupsInput input, MeetupEvent meetupEvent)
        {
            return new TravelTimeInput
            {
                EventName = meetupEvent.Name,
                GroupName = meetupEvent.Group.Name,
                EventStartUnixTimeSeconds = meetupEvent.UnixTimeMilliseconds / 1000,
                TravelMode = input.TravelMode,
                DepartureAddress = input.DepartureAddress,
                DestinationAddress = $"{meetupEvent.Venue.Name}, {meetupEvent.Venue.Address}, {meetupEvent.Venue.City}",
                TrafficModel = "pessimistic"
            };
        }
    }
}
