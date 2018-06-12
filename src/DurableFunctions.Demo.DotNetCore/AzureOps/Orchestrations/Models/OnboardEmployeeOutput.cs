using DurableFunctions.Demo.DotNetCore.AzureOps.DomainModels;

namespace DurableFunctions.Demo.DotNetCore.AzureOps.Orchestrations.Models
{
    public sealed class OnboardEmployeeOutput
    {        
        public CreatedResource[] CreatedResources { get; set; }
    }
}
