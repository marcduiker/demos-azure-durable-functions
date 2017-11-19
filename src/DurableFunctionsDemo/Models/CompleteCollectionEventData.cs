namespace DurableFunctionsDemo.Models
{
    public class CompleteCollectionEventData
    {
        public string OrchestrationInstanceId { get; set; }

        public string EventName => "isCompleted";

        public bool IsCompleted { get; set; }

    }
}
