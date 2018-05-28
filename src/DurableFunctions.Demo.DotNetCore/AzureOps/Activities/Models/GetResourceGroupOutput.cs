using Microsoft.Azure.Management.ResourceManager.Fluent.Models;

namespace DurableFunctions.Demo.DotNetCore.AzureOps.Activities.Models
{
    public sealed class GetResourceGroupOutput
    {
        public ResourceGroupInner ResourceGroup { get; set; }
    }
}
