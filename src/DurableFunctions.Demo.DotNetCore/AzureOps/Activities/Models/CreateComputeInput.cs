namespace DurableFunctions.Demo.DotNetCore.AzureOps.Activities.Models
{
    public sealed class CreateComputeInput
    {
        public string Region { get; set; }
        
        public string ResourceGroup { get; set; }

        public string VMName { get; set; }

        public VMConfiguration VMConfiguration { get; set; }
    }
}

