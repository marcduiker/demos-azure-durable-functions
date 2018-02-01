namespace DurableFunctionsDemo.MeetupTravelInfo.Models
{
    public class TravelTimeInput
    {
        public string DepartureAddress { get; set; }

        public string DestinationAddress { get; set; }

        public string TravelMode { get; set; }

        public string TrafficModel { get; set; }

        public long EventStartUnixTimeSeconds { get; set; }

        public string EventName { get; set; }

        public string GroupName { get; set; }
    }
}
