using DurableFunctions.Demo.DotNetCore.AzureOps.DomainModels;

namespace DurableFunctions.Demo.DotNetCore.AzureOps.Activities.Models
{
    public sealed class CreateWebAppOutput
    {
        public CreatedResource CreatedResource { get; set; }
    }
}
