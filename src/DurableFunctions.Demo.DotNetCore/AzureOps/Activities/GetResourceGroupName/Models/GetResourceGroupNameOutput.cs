using DurableFunctions.Demo.DotNetCore.AzureOps.Models;

namespace DurableFunctions.Demo.DotNetCore.AzureOps.Activities.GetResourceGroupName.Models
{
    public sealed class GetResourceGroupNameOutput
    {
        public Environment Environment { get; set; }

        public string ResourceGroup { get; set; }
    }
}
