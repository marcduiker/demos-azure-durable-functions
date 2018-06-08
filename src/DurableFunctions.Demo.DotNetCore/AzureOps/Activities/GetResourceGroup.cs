using System.Threading.Tasks;
using DurableFunctions.Demo.DotNetCore.AzureOps.Helpers;
using Microsoft.Azure.Management.ResourceManager.Fluent.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.Demo.DotNetCore.AzureOps.Activities
{
    public static class GetResourceGroup
    {
        [FunctionName(nameof(GetResourceGroup))]
        public static async Task<ResourceGroupInner> Run(
            [ActivityTrigger] DurableActivityContext activityContext,
            ILogger logger)
        {
            string resourceGroup = activityContext.GetInput<string>();
            var resourceGroupFound = await AzureManagement.Instance.Authenticated.ResourceGroups.GetByNameAsync(resourceGroup);

            return resourceGroupFound.Inner;
        }
    }
}
