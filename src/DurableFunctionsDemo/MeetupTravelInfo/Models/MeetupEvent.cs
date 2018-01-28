using Newtonsoft.Json;

namespace DurableFunctionsDemo.MeetupTravelInfo.Models
{
    public class MeetupEvent
    {
        [JsonProperty("time")]
        public long EpochTime { get; set; }

        [JsonProperty("local_date")]
        public string LocalDate { get; set; }

        [JsonProperty("local_time")]
        public string LocalTime { get; set; }

        public Venue Venue { get; set; }
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
