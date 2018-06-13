using DurableFunctions.Demo.DotNetCore.AzureOps.Models;

namespace DurableFunctions.Demo.DotNetCore.AzureOps.Orchestrations.Models
{
    public sealed class OnboardEmployeeOutput
    {        
        public CreatedResource[] CreatedResources { get; set; }
    }
}
