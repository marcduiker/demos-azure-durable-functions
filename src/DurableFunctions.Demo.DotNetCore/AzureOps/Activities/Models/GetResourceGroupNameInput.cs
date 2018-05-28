using DurableFunctions.Demo.DotNetCore.AzureOps.DomainModels;

namespace DurableFunctions.Demo.DotNetCore.AzureOps.Activities.Models
{
    public sealed class GetResourceGroupNameInput
    {
        public string UserThreeLetterCode { get; set; }
        public Environment Environment { get; set; }
    }
}
