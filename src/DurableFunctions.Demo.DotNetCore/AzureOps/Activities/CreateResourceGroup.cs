using System;
using System.Threading.Tasks;
using DurableFunctions.Demo.DotNetCore.AzureOps.Activities.Models;
using DurableFunctions.Demo.DotNetCore.AzureOps.Helpers;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.Demo.DotNetCore.AzureOps.Activities
{
    public static class CreateResourceGroup
    {
        [FunctionName(nameof(CreateResourceGroup))]
        public static async Task<CreateResourceGroupOutput> Run(
            [ActivityTrigger] DurableActivityContext activityContext,
            ILogger logger)
        {
            var input = activityContext.GetInput<CreateResourceGroupInput>();

            try
            {
                var resourceGroupToCreate = await AzureManagement.Instance.Authenticated.ResourceGroups
                    .Define(input.ResourceGroupName)
                    .WithRegion(input.Region)
                    .WithTags(input.Tags)
                    .CreateAsync();

                return new CreateResourceGroupOutput { ResourceGroupId = resourceGroupToCreate.Inner.Id };
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw;
            }
        }
    }
}
