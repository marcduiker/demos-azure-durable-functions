using DurableFunctions.Demo.DotNetCore.AzureOps.DomainModels;

namespace DurableFunctions.Demo.DotNetCore.AzureOps.Activities.Models
{
    public sealed class GetResourceGroupNameOutput
    {
        public Environment Environment { get; set; }

        public string ResourceGroup { get; set; }
    }
}
