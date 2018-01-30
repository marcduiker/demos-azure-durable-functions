using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using DurableFunctionsDemo.MeetupTravelInfo.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json.Linq;

namespace DurableFunctionsDemo.MeetupTravelInfo.ActivityFunctions
{
    public static class GetTravelTime
    {
        [FunctionName("GetTravelTime")]
        public static async Task<TravelInfo> Run(
            [ActivityTrigger]DurableActivityContext activityContext,
            TraceWriter log)
        {
            var input = activityContext.GetInput<TravelTimeInput>();

            string endpointUri = ConstructDirectionsUri(input);

            var httpClient = new HttpClient();
            var result = await httpClient.GetAsync(endpointUri);
            var directionResult = result.Content.ReadAsStringAsync().Result;

            var travelDurationToken = JToken.Parse(directionResult).SelectToken("routes[0].legs[0].duration_in_traffic");
            int travelDurationSeconds = travelDurationToken.Value<int>("value");

            int bufferSeconds =  Convert.ToInt32(TimeSpan.FromMinutes(10).TotalSeconds);
            long departureUnixTime = input.EventStartUnixTimeSeconds - travelDurationSeconds - bufferSeconds;
            int durationSeconds = travelDurationSeconds + bufferSeconds;

            return CreateTravelInfo(departureUnixTime, durationSeconds, input);
        }

        private static TravelInfo CreateTravelInfo(long departureUnixTime, int durationSeconds, TravelTimeInput input)
        {
            return new TravelInfo
            {
                DepartureTime = departureUnixTime.FromUnixTime().ToLocalTime().ToString("F"),
                DepartureUnixTimeSeconds = departureUnixTime,
                Destination = input.DestinationAddress,
                DurationSeconds = durationSeconds,
                DurationText = $"{durationSeconds / 60} minutes",
                EventName = input.EventName,
                GroupName = input.GroupName
            };
        }

        public static DateTime FromUnixTime(this long unixTimeSeconds)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTimeSeconds);
        }

        public static long ToUnixTime(this DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date - epoch).TotalSeconds);
        }

        private static string ConstructDirectionsUri(TravelTimeInput input)
        {
            string directionsUri = Environment.GetEnvironmentVariable("GoogleDirectionsBaseUri");
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["origin"] = input.DepartureAddress;
            queryString["destination"] = input.DestinationAddress;
            queryString["mode"] = input.TravelMode;
            queryString["traffic_model"] = input.TrafficModel;
            queryString["departure_time"] = input.EventStartUnixTimeSeconds.ToString();
            queryString["key"] = Environment.GetEnvironmentVariable("GoogleApiKey");

            return $"{directionsUri}json?{queryString}";
        }
    }
}
