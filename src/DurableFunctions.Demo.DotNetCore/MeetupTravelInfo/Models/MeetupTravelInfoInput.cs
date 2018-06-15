namespace DurableFunctions.Demo.DotNetCore.MeetupTravelInfo.Models
{
    public class MeetupTravelInfoInput
    {
        public string MeetupGroupUrlName { get; set; }

        public string DepartureAddress { get; set; }

        public string TravelMode { get; set; }
    }
}
