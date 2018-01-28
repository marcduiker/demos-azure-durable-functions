namespace DurableFunctionsDemo.MeetupTravelInfo.Models
{
    public class MeetupTravelInfoInput
    {
        public string MeetupGroupUrlName { get; set; }

        public string OriginAddress { get; set; }

        public string TravelMode { get; set; }
    }
}
