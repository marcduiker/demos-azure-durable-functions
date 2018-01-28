using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using DurableFunctionsDemo.BestMeetupFinder.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DurableFunctionsDemo.BestMeetupFinder.ActivityFunctions
{
    public static class FindMeetups
    {
        [FunctionName("FindMeetups")]
        public static async Task<MeetupEvent[]> Run(
            [ActivityTrigger]DurableActivityContext activityContext,
            TraceWriter log)
        {
            var input = activityContext.GetInput<FindMeetupsInput>();
            
            var httpClient = new HttpClient();
            string upcomingEventsUri = "https://api.meetup.com/find/upcoming_events";

            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["start_date_range"] =  $"{DateTime.Today:yyyy-MM-dd}T00:00";
            queryString["end_date_range"] = $"{DateTime.Today.AddDays(input.WithinNumberOfDays):yyyy-MM-dd}T23:59";
            queryString["page"] = "20";
            queryString["sign"] = "true";
            queryString["key"] = "";
            queryString["text"] = input.SearchText;

            var endpointUri = $"{upcomingEventsUri}?{queryString}";
            var result = await httpClient.GetAsync(endpointUri);
            var jsonResult = result.Content.ReadAsStringAsync().Result;

            var events = JToken.Parse(jsonResult).SelectToken("events");

            var meetupEvents = JsonConvert.DeserializeObject<MeetupEvent[]>(events.ToString());

            return meetupEvents;
        }
    }
}
