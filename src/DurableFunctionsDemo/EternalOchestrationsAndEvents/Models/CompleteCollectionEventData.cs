namespace DurableFunctionsDemo.EternalOchestrationsAndEvents.Models
{
    public class CompleteCollectionEventData
    {
        public string OrchestrationInstanceId { get; set; }

        public bool IsCompleted { get; set; }
    }
}
