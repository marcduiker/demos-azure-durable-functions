namespace DurableFunctions.Demo.DotNetCore.MeetupTravelInfo.Models
{
    public class TravelInfo
    {
        public string EventName { get; set; }

        public string GroupName { get; set; }

        public long DepartureUnixTimeSeconds { get; set; }

        public string DepartureTime { get; set; }

        public string DurationText { get; set; }

        public int DurationSeconds { get; set; }

        public string Destination { get; set; }
    }
}
