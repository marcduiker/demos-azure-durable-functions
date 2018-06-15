using System;
using System.Collections.Generic;
using DurableFunctions.Demo.DotNetCore.AzureOps.Activities.GetRegionAndCountryCode.Models;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.Demo.DotNetCore.AzureOps.Activities.GetRegionAndCountryCode
{
    public static class GetRegionAndCountryCode
    {
        [FunctionName(nameof(GetRegionAndCountryCode))]
        public static GetRegionAndCountryCodeOutput Run(
            [ActivityTrigger] GetRegionAndCountryCodeInput input,
            ILogger logger)
        {
            var result = new GetRegionAndCountryCodeOutput();
            if (LocationRegionDictionary.TryGetValue(input.UserLocation, out Tuple<CountryISOCode, Region> countryAndRegion))
            {
                result.CountryIsoCode = countryAndRegion.Item1.Value;
                result.Region = countryAndRegion.Item2.Name;
            }
            else
            {
                // The default
                result.CountryIsoCode = CountryISOCode.Netherlands.Value;
                result.Region = Region.EuropeWest.Name;
            }

            return result;
        }

        private static readonly Dictionary<string, Tuple<CountryISOCode, Region>> LocationRegionDictionary = new Dictionary<string, Tuple<CountryISOCode, Region>>
        {
            { "Amsterdam", Tuple.Create(CountryISOCode.Netherlands, Region.EuropeWest) },
            { "New York", Tuple.Create(CountryISOCode.UnitedStates, Region.USEast) },
            { "Tokyo", Tuple.Create(CountryISOCode.Japan, Region.JapanEast) }
        };
        
    }
}
