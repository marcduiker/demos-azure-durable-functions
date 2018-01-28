using System;
using Newtonsoft.Json;

namespace DurableFunctionsDemo.BestMeetupFinder.Models
{
    public class MeetupEvent
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("group")]
        private Group Group { get; set; }

        [JsonIgnore]
        public string GroupName => Group.Name;

        [JsonProperty("local_date")]
        public string LocalDate { get; set; }

        [JsonProperty("local_time")]
        public string LocalTime { get; set; }

        [JsonProperty("venue")]
        public Venue Venue { get; set; }
    }

    public class Group
    {
        [JsonProperty("name")]
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
