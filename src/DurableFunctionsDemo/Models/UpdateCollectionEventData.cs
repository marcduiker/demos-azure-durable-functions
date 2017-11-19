namespace DurableFunctionsDemo.Models
{
    public class UpdateCollectionEventData
    {
        public string OrchestrationInstanceId { get; set; }

        public string EventName { get; set; }

        public string Name { get; set; }
    }
}
