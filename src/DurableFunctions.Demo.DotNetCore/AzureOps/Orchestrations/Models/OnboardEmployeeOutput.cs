namespace DurableFunctions.Demo.DotNetCore.AzureOps.Orchestrations.Models
{
    public sealed class OnboardEmployeeOutput
    {        
        public string[] ResourceGroupIDs { get; set; }

        public string[] WebAppIDs { get; set; }

    }
}
