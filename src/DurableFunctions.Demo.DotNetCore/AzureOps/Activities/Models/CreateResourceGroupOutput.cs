using DurableFunctions.Demo.DotNetCore.AzureOps.DomainModels;

namespace DurableFunctions.Demo.DotNetCore.AzureOps.Activities.Models
{
    public sealed class CreateResourceGroupOutput
    {
        public CreatedResource CreatedResource { get; set; }
    }
}
