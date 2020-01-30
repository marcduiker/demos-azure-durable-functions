namespace DurableFunctions.Demo.ScreenShots.Models
{
    public class Approval
    {
        public Approval(string eventName, bool isApproved)
        {
            EventName = eventName;
            IsApproved = isApproved;
        }

        public string EventName { get; set; }
        
        public bool IsApproved { get; set; }
    }
}