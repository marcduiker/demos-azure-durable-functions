using DurableFunctions.Demo.DotNetCore.AzureOps.Activities.Models;

namespace DurableFunctions.Demo.DotNetCore.AzureOps.Models
{
    public sealed class OnboardEmployeeOutput
    {
        public string[] ResourceGroupIDs { get; set; }

        public VMConfiguration VMConfiguration { get; set; }
    }
}
