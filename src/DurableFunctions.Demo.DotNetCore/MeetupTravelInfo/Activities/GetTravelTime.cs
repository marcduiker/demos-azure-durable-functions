using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using DurableFunctions.Demo.DotNetCore.MeetupTravelInfo.Models;
using DurableTask.Core.Exceptions;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json.Linq;

namespace DurableFunctions.Demo.DotNetCore.MeetupTravelInfo.Activities
{
    public static class GetTravelTime
    {
        private static readonly HttpClient HttpClient = new HttpClient();

        [FunctionName(nameof(GetTravelTime))]
        public static async Task<TravelInfo> Run(
            [ActivityTrigger]DurableActivityContext activityContext,
            TraceWriter log)
        {
            var input = activityContext.GetInput<TravelTimeInput>();
            string endpointUri = ConstructDirectionsUri(input);

            var result = await HttpClient.GetAsync(endpointUri);
            var directionResult = await result.Content.ReadAsStringAsync();
            try
            {
                var travelDurationToken = JToken.Parse(directionResult)
                    .SelectToken("routes[0].legs[0].duration_in_traffic");
                int travelDurationSeconds = travelDurationToken.Value<int>("value");

                int bufferSeconds = Convert.ToInt32(TimeSpan.FromMinutes(10).TotalSeconds);
                int durationSeconds = travelDurationSeconds + bufferSeconds;
                long departureUnixTime = input.EventStartUnixTimeSeconds - durationSeconds;

                return CreateTravelInfo(departureUnixTime, durationSeconds, input);
            }
            catch (Exception e)
            {
                string error = JToken.Parse(directionResult).SelectToken("error_message").Value<string>();
                log.Error(error, e);
                throw new TaskFailedException(error, e);
            }
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
