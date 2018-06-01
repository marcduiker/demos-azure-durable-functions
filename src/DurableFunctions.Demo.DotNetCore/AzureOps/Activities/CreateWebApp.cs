using System;
using System.Threading.Tasks;
using DurableFunctions.Demo.DotNetCore.AzureOps.Activities.Models;
using DurableFunctions.Demo.DotNetCore.AzureOps.Helpers;
using Microsoft.Azure.Management.AppService.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Environment = DurableFunctions.Demo.DotNetCore.AzureOps.DomainModels.Environment;

namespace DurableFunctions.Demo.DotNetCore.AzureOps.Activities
{
    public static class CreateWebApp
    {
        [FunctionName(nameof(CreateWebApp))]
        public static async Task<CreateWebAppOutput> Run(
            [ActivityTrigger] DurableActivityContext activityContext,
            TraceWriter logger)
        {
            var input = activityContext.GetInput<CreateWebAppInput>();

            try
            {
                var appName = GetWebAppName(input.UserThreeLetterCode, input.Region, input.Environment);

                await AzureManagement.Instance.Authenticated.WebApps
                    .Define(appName)
                    .WithRegion(input.Region)
                    .WithExistingResourceGroup(input.ResourceGroupName)
                    .WithNewWindowsPlan(PricingTier.BasicB1)
                    .CreateAsync();
                
                return new CreateWebAppOutput
                {
                    AppName = appName
                };

            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw;
            }
        }

        private static string GetWebAppName(string userThreeLetterCode, string region, Environment environment)
        {
            return $"{userThreeLetterCode}-{region}-{environment}-app".ToLower();
        }
    }
}