using System.Collections.Generic;
using DurableFunctions.Demo.DotNetCore.AzureOps.DomainModels;

namespace DurableFunctions.Demo.DotNetCore.AzureOps.Activities.Models
{
    public sealed class CreateWebAppInput
    {
        public string Region { get; set; }

        public string ResourceGroupName { get; set; }

        public string UserThreeLetterCode { get; set; }

        public Environment Environment { get; set; }

        public Dictionary<string, string> Tags { get; set; }
    }
}
