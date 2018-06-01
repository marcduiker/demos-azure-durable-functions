using System;
using System.Threading.Tasks;
using DurableFunctions.Demo.DotNetCore.AzureOps.Activities.Models;
using DurableFunctions.Demo.DotNetCore.AzureOps.Helpers;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace DurableFunctions.Demo.DotNetCore.AzureOps.Activities
{
    public static class AddUser
    {
        const string DefaultPassWord = "WorkingWithMegacorpIsAwsome";
        const string CompanyDomain = "azureops.onmicrosoft.com";

        [FunctionName(nameof(AddUser))]
        public static async Task<AddUserOutput> Run(
            [ActivityTrigger] DurableActivityContext activityContext,
            TraceWriter logger)
        {
            var input = activityContext.GetInput<AddUserInput>();

            try
            {
                var user = await AzureManagement.Instance.Authenticated.AccessManagement
                    .ActiveDirectoryUsers
                    .Define(input.UserName)
                    .WithEmailAlias(GetEmailAddress(input.UserName))
                    .WithPassword(DefaultPassWord)
                    .WithPromptToChangePasswordOnLogin(true)
                    .WithUsageLocation(CountryISOCode.Parse(input.CountryIsoCode))
                    .CreateAsync();

                return new AddUserOutput
                {
                    Email = user.Mail
                };

            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw;
            }
        }

        private static string GetEmailAddress(string userName)
        {
            return $"{userName}@{CompanyDomain}";
        }
    }
}