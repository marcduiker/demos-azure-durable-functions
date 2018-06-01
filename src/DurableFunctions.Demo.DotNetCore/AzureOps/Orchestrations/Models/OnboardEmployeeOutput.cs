namespace DurableFunctions.Demo.DotNetCore.AzureOps.Orchestrations.Models
{
    public sealed class OnboardEmployeeOutput
    {
        public string Email { get; set; }
        
        public string[] ResourceGroupIDs { get; set; }

    }
}
