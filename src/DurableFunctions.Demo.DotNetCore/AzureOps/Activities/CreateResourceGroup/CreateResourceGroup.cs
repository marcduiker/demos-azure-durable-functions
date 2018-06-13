using System;
using System.Threading.Tasks;
using DurableFunctions.Demo.DotNetCore.AzureOps.Activities.CreateResourceGroup.Models;
using DurableFunctions.Demo.DotNetCore.AzureOps.Helpers;
using DurableFunctions.Demo.DotNetCore.AzureOps.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.Demo.DotNetCore.AzureOps.Activities.CreateResourceGroup
{
    public static class CreateResourceGroup
    {
        [FunctionName(nameof(CreateResourceGroup))]
        public static async Task<CreateResourceGroupOutput> Run(
            [ActivityTrigger] CreateResourceGroupInput input,
            ILogger logger)
        {
            try
            {
                var resourceGroup = await AzureManagement.Instance.Authenticated.ResourceGroups
                    .Define(input.ResourceGroupName)
                    .WithRegion(input.Region)
                    .WithTags(input.Tags)
                    .CreateAsync();

                return new CreateResourceGroupOutput { CreatedResource = new CreatedResource
                {
                    Id = resourceGroup.Id,
                    Name = resourceGroup.Name,
                    Region = resourceGroup.Region.Name
                }};
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw;
            }
        }
    }
}
