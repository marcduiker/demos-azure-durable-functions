using System;
namespace DurableFunctionsDemo.MeetupTravelInfo.Models
{
    public class TravelInfo
    {
        public long DepartureUnixTimeSeconds { get; set; }

        public string DepartureTime { get; set; }

        public string Duration { get; set; }

        public string Destination { get; set; }
    }
}
