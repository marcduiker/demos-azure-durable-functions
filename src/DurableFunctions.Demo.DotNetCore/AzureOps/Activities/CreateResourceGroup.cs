using System;
using System.Threading.Tasks;
using DurableFunctions.Demo.DotNetCore.AzureOps.Activities.Models;
using DurableFunctions.Demo.DotNetCore.AzureOps.Helpers;
using Microsoft.Azure.Management.ResourceManager.Fluent.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace DurableFunctions.Demo.DotNetCore.AzureOps.Activities
{
    public static class CreateResourceGroup
    {
        [FunctionName(nameof(CreateResourceGroup))]
        public static async Task<ResourceGroupInner> Run(
            [ActivityTrigger] DurableActivityContext activityContext,
            TraceWriter logger)
        {
            var input = activityContext.GetInput<CreateResourceGroupInput>();

            try
            {
                var resourceGroupToCreate = await AzureManagement.Instance.AzureApi.ResourceGroups
                    .Define(input.ResourceGroupName)
                    .WithRegion(input.Region)
                    .CreateAsync();

                return resourceGroupToCreate.Inner;
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw;
            }
        }
    }
}
