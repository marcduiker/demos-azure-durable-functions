using System.Collections.Generic;
using DurableFunctions.Demo.DotNetCore.AzureOps.Activities.CreateResourceGroup.Models;
using DurableFunctions.Demo.DotNetCore.AzureOps.Activities.CreateWebApp.Models;
using DurableFunctions.Demo.DotNetCore.AzureOps.Activities.GetRegionAndCountryCode.Models;
using DurableFunctions.Demo.DotNetCore.AzureOps.Activities.GetResourceGroupName.Models;
using DurableFunctions.Demo.DotNetCore.AzureOps.Models;

namespace DurableFunctions.Demo.DotNetCore.AzureOps.Helpers
{
    public static class Builder
    {
        public static GetRegionAndCountryCodeInput CreateRegionAndCountryCodeInput(string userLocation)
        {
            return new GetRegionAndCountryCodeInput
            {
                UserLocation = userLocation
            };
        }

        public static GetResourceGroupNameInput CreateGetResourceGroupNameInput(
            Environment environment,
            string userThreeLetterCode)
        {
            return new GetResourceGroupNameInput
            {
                Environment = environment,
                UserThreeLetterCode = userThreeLetterCode
            };
        }
        public static CreateResourceGroupInput CreateResourceGroupInput(
            GetRegionAndCountryCodeOutput regionAndCountry,
            KeyValuePair<string, Environment> resourceGroupAndEnvironment,
            string userName
        )
        {
            return new CreateResourceGroupInput
            {
                Region = regionAndCountry.Region,
                ResourceGroupName = resourceGroupAndEnvironment.Key,
                Tags = GetTags(userName, resourceGroupAndEnvironment.Value)
            };
        }

        public static CreateWebAppInput CreateWebAppInput(
            GetRegionAndCountryCodeOutput regionAndCountry,
            KeyValuePair<string, Environment> resourceGroupAndEnvironment,
            string userThreeLetterCode,
            string userName
        )
        {
            return new CreateWebAppInput
            {
                Region = regionAndCountry.Region,
                Environment = resourceGroupAndEnvironment.Value,
                ResourceGroupName = resourceGroupAndEnvironment.Key,
                UserThreeLetterCode = userThreeLetterCode,
                Tags = GetTags(userName, resourceGroupAndEnvironment.Value)
            };
        }

        private static Dictionary<string, string> GetTags(string userName, Environment environment)
        {
            return new Dictionary<string, string>
            {
                { "user", userName},
                { "environment", environment.ToString("G")}
            };
        }
    }
}
