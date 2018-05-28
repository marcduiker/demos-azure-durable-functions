using DurableFunctions.Demo.DotNetCore.AzureOps.DomainModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DurableFunctions.Demo.DotNetCore.AzureOps.Orchestrations.Models
{
    public sealed class OnboardEmployeeInput
    {
        public string UserName { get; set; }

        public string UserThreeLetterCode { get; set; }

        public string Role { get; set; }

        public string Location { get; set; }

        [JsonProperty(ItemConverterType=typeof(StringEnumConverter))]
        public Environment[] Environments { get; set; }
    }
}
