using System.Collections.Generic;
using DurableFunctions.Demo.DotNetCore.AzureOps.Activities.Models;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace DurableFunctions.Demo.DotNetCore.AzureOps.Activities
{
    public static class GetAzureRegion
    {
        [FunctionName(nameof(GetAzureRegion))]
        public static GetAzureRegionOutput Run(
            [ActivityTrigger] DurableActivityContext activityContext,
            TraceWriter logger)
        {
            var input = activityContext.GetInput<GetAzureRegionInput>();
            var result = new GetAzureRegionOutput();
            if (LocationRegionDictionary.TryGetValue(input.UserLocation, out Region region))
            {
                result.Region = region.Name;
            }
            else
            {
                // The default
                result.Region = Region.EuropeWest.Name;
            }

            return result;
        }

        private static readonly Dictionary<string, Region> LocationRegionDictionary = new Dictionary<string, Region>
        {
            { "Amsterdam", Region.EuropeWest },
            { "New York", Region.USEast },
            { "Tokyo", Region.JapanEast }
        };
        
    }
}
