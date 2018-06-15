namespace DurableFunctions.Demo.DotNetCore.MeetupTravelInfo.Models
{
    public class FindClosestMeetupsInput
    {
        public string DepartureAddress { get; set; }

        public int MaxNumberOfEvents { get; set; }

        public string SearchText { get; set; }

        public string TravelMode { get; set; }
    }
}
