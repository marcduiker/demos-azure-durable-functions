using Newtonsoft.Json;

namespace DurableFunctions.Demo.DotNetCore.MeetupTravelInfo.Models
{
    public class MeetupEvent
    {
        public string Name { get; set; }

        public Group Group { get; set; }

        [JsonProperty("time")]
        public long UnixTimeMilliseconds { get; set; }

        [JsonProperty("local_date")]
        public string LocalDate { get; set; }

        [JsonProperty("local_time")]
        public string LocalTime { get; set; }

        public Venue Venue { get; set; }
    }

    public class Group
    {
        public string Name { get; set; }
    }

    public class Venue
    {
        public string Name { get; set; }

        public string City { get; set; }

        [JsonProperty("address_1")]
        public string Address { get; set; }

        [JsonProperty("lon")]
        public double Longitude { get; set; }

        [JsonProperty("lat")]
        public double Lattitude { get; set; }
    }
}
