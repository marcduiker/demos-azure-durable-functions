using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using DurableFunctions.Demo.DotNetCore.MeetupTravelInfo.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace DurableFunctions.Demo.DotNetCore.MeetupTravelInfo.Activities
{
    public static class GetUpcomingEventsByText
    {
        [FunctionName(nameof(GetUpcomingEventsByText))]
        public static async Task<MeetupEvent[]> Run(
            [ActivityTrigger]DurableActivityContext activityContext,
            ILogger log)
        {
            var input = activityContext.GetInput<FindClosestMeetupsInput>();

            string endpointUri = ConstructEventUri(input);

            var httpClient = new HttpClient();
            var result = await httpClient.GetAsync(endpointUri);
            var contentResult = result.Content.ReadAsStringAsync().Result;
            var meetupEvents = JToken.Parse(contentResult).SelectToken("events").ToObject<MeetupEvent[]>();

            return meetupEvents;
        }

        private static string ConstructEventUri(FindClosestMeetupsInput input)
        {
            string meetupGroupUri = Environment.GetEnvironmentVariable("MeetupBaseUri");
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["text"] = input.SearchText;
            queryString["page"] = input.MaxNumberOfEvents.ToString();
            queryString["sign"] = "true";
            queryString["key"] = Environment.GetEnvironmentVariable("MeetupApiKey");

            return $"{meetupGroupUri}/find/upcoming_events?{queryString}";
        }
    }
}
