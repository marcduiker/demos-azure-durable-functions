using System;
using System.Threading.Tasks;
using DurableFunctions.Demo.DotNetCore.AzureOps.Activities.Models;
using DurableFunctions.Demo.DotNetCore.AzureOps.DomainModels;
using DurableFunctions.Demo.DotNetCore.AzureOps.Helpers;
using Microsoft.Azure.Management.AppService.Fluent;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Environment = DurableFunctions.Demo.DotNetCore.AzureOps.DomainModels.Environment;

namespace DurableFunctions.Demo.DotNetCore.AzureOps.Activities
{
    public static class CreateWebApp
    {
        [FunctionName(nameof(CreateWebApp))]
        public static async Task<CreateWebAppOutput> Run(
            [ActivityTrigger] DurableActivityContext activityContext,
            ILogger logger)
        {
            var input = activityContext.GetInput<CreateWebAppInput>();

            try
            {
                var appName = GetWebAppName(input.UserThreeLetterCode, input.Region, input.Environment);

                var webApp = await AzureManagement.Instance.Authenticated.WebApps
                    .Define(appName)
                    .WithRegion(input.Region)
                    .WithExistingResourceGroup(input.ResourceGroupName)
                    .WithNewWindowsPlan(PricingTier.BasicB1)
                    .WithTags(input.Tags)
                    .CreateAsync();
               

                return new CreateWebAppOutput
                {
                    CreatedResource = new CreatedResource
                    {

                        Id = webApp.Id,
                        Name = webApp.Name,
                        ResourceGroup = webApp.ResourceGroupName
                    }
                };

            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw;
            }
        }

        private static string GetWebAppName(string userThreeLetterCode, string region, Environment environment)
        {
            return $"{userThreeLetterCode}-{region}-{environment}-app".ToLower();
        }
    }
}