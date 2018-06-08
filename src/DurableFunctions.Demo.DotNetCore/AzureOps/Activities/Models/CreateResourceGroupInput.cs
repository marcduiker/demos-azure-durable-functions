using System.Collections.Generic;

namespace DurableFunctions.Demo.DotNetCore.AzureOps.Activities.Models
{
    public sealed class CreateResourceGroupInput
    {
        public string ResourceGroupName { get; set; }

        public string Region { get; set; }

        public Dictionary<string, string> Tags { get; set; }
    }
}
