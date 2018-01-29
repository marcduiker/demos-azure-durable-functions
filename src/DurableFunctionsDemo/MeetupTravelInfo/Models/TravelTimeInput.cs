namespace DurableFunctionsDemo.MeetupTravelInfo.Models
{
    public class TravelTimeInput
    {
        public string OriginAddress { get; set; }

        public string DestinationAddress { get; set; }

        public string TravelMode { get; set; }

        public string TrafficModel { get; set; }

        public long EventStartUnixTimeSeconds { get; set; }
    }
}
