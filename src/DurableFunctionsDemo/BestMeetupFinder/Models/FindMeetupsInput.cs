namespace DurableFunctionsDemo.BestMeetupFinder.Models
{
    public class FindMeetupsInput
    {
        /// <summary>
        /// Search text to find meetups.
        /// </summary>
        public string SearchText { get; set; }

        /// <summary>
        /// Origin is used as the starting point for the route to the meetup.
        /// </summary>
        public string Origin { get; set; }

        /// <summary>
        /// Upper limit of days from current date to find meetups.
        /// </summary>
        public int WithinNumberOfDays { get; set; }

        /// <summary>
        /// Maximum travel time in minutes to the meetup.
        /// </summary>
        public int MaxTravelTime { get; set; }

        /// <summary>
        /// Travel mode used for calculating route & duration to the meetup.
        /// Accepted values: driving, walking, transit
        /// </summary>
        public string TravelMode { get; set; }
    }
}
