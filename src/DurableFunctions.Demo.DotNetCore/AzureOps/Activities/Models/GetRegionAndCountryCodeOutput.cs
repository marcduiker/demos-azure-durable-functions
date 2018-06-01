using Microsoft.Azure.Management.ResourceManager.Fluent.Core;

namespace DurableFunctions.Demo.DotNetCore.AzureOps.Activities.Models
{
    public sealed class GetRegionAndCountryCodeOutput
    {
        public string CountryIsoCode { get; set; }
        public string Region { get; set; }
    }
}
