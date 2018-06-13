using DurableFunctions.Demo.DotNetCore.AzureOps.Models;

namespace DurableFunctions.Demo.DotNetCore.AzureOps.Activities.CreateResourceGroup.Models
{
    public sealed class CreateResourceGroupOutput
    {
        public CreatedResource CreatedResource { get; set; }
    }
}
