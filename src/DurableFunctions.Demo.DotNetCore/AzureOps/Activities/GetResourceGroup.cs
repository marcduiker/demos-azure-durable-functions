using System.Threading.Tasks;
using DurableFunctions.Demo.DotNetCore.AzureOps.Helpers;
using Microsoft.Azure.Management.ResourceManager.Fluent.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace DurableFunctions.Demo.DotNetCore.AzureOps.Activities
{
    public static class GetResourceGroup
    {
        [FunctionName(nameof(GetResourceGroup))]
        public static async Task<ResourceGroupInner> Run(
            [ActivityTrigger] DurableActivityContext activityContext,
            TraceWriter logger)
        {
            string resourceGroup = activityContext.GetInput<string>();
            var resourceGroupFound = await AzureManagement.Instance.AzureApi.ResourceGroups.GetByNameAsync(resourceGroup);

            return resourceGroupFound.Inner;
        }
    }
}
