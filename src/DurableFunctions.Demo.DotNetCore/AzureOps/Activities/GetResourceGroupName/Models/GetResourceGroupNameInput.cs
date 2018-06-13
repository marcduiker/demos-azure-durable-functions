using DurableFunctions.Demo.DotNetCore.AzureOps.Models;

namespace DurableFunctions.Demo.DotNetCore.AzureOps.Activities.GetResourceGroupName.Models
{
    public sealed class GetResourceGroupNameInput
    {
        public string UserThreeLetterCode { get; set; }
        public Environment Environment { get; set; }
    }
}
