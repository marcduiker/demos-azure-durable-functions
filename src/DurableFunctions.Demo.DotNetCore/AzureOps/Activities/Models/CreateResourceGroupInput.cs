using Microsoft.Azure.Management.ResourceManager.Fluent.Core;

namespace DurableFunctions.Demo.DotNetCore.AzureOps.Activities.Models
{
    public sealed class CreateResourceGroupInput
    {
        public string ResourceGroupName { get; set; }

        public string Region { get; set; }
    }
}
