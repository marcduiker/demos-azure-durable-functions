using DurableFunctions.Demo.DotNetCore.AzureOps.Models;

namespace DurableFunctions.Demo.DotNetCore.AzureOps.Activities.CreateWebApp.Models
{
    public sealed class CreateWebAppOutput
    {
        public CreatedResource CreatedResource { get; set; }
    }
}
