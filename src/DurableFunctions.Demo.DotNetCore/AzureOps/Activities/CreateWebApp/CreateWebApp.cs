using System;
using System.Threading.Tasks;
using DurableFunctions.Demo.DotNetCore.AzureOps.Activities.CreateWebApp.Models;
using DurableFunctions.Demo.DotNetCore.AzureOps.Models;
using DurableFunctions.Demo.DotNetCore.AzureOps.Helpers;
using Microsoft.Azure.Management.AppService.Fluent;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Environment = DurableFunctions.Demo.DotNetCore.AzureOps.Models.Environment;

namespace DurableFunctions.Demo.DotNetCore.AzureOps.Activities.CreateWebApp
{
    public static class CreateWebApp
    {
        private static IWebApps _webApps;

        public static IWebApps WebApps
        {
            get { return _webApps = _webApps ?? AzureManagement.Instance.Authenticated.WebApps; }
            set => _webApps = value;
        }

        [FunctionName(nameof(CreateWebApp))]
        public static async Task<CreateWebAppOutput> Run(
            [ActivityTrigger] CreateWebAppInput input,
            ILogger logger)
        {
            try
            {
                var appName = GetWebAppName(input.UserThreeLetterCode, input.Region, input.Environment);

                var webApp = await WebApps
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