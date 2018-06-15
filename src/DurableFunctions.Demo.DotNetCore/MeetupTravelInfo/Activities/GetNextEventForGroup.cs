using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using DurableFunctions.Demo.DotNetCore.MeetupTravelInfo.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.Demo.DotNetCore.MeetupTravelInfo.Activities
{
    public static class GetNextEventForGroup
    {
        private static readonly HttpClient HttpClient = new HttpClient();

        [FunctionName(nameof(GetNextEventForGroup))]
        public static async Task<MeetupEvent> Run(
            [ActivityTrigger]DurableActivityContext activityContext,
            ILogger log)
        {
            var input = activityContext.GetInput<MeetupTravelInfoInput>();
            string endpointUri = ConstructEventUri(input);
            
            var result = await HttpClient.GetAsync(endpointUri);
            var meetupEvents = await result.Content.ReadAsAsync<MeetupEvent[]>();

            return meetupEvents.FirstOrDefault();
        }

        private static string ConstructEventUri(MeetupTravelInfoInput input)
        {
            string meetupGroupUri = Environment.GetEnvironmentVariable("MeetupBaseUri");
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["page"] = "1";
            queryString["sign"] = "true";
            queryString["key"] = Environment.GetEnvironmentVariable("MeetupApiKey");

            return $"{meetupGroupUri}/{input.MeetupGroupUrlName}/events/?{queryString}";
        }
    }
}
