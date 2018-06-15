using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DurableFunctions.Demo.DotNetCore.AzureOps.Activities.CreateResourceGroup;
using DurableFunctions.Demo.DotNetCore.AzureOps.Activities.CreateResourceGroup.Models;
using DurableFunctions.Demo.DotNetCore.AzureOps.Activities.CreateWebApp;
using DurableFunctions.Demo.DotNetCore.AzureOps.Activities.CreateWebApp.Models;
using DurableFunctions.Demo.DotNetCore.AzureOps.Activities.GetRegionAndCountryCode;
using DurableFunctions.Demo.DotNetCore.AzureOps.Activities.GetRegionAndCountryCode.Models;
using DurableFunctions.Demo.DotNetCore.AzureOps.Activities.GetResourceGroupName;
using DurableFunctions.Demo.DotNetCore.AzureOps.Activities.GetResourceGroupName.Models;
using DurableFunctions.Demo.DotNetCore.AzureOps.Activities.Models;
using DurableFunctions.Demo.DotNetCore.AzureOps.Activities.SendSlackNotification;
using DurableFunctions.Demo.DotNetCore.AzureOps.Activities.ValidateOrchestrationInput;
using DurableFunctions.Demo.DotNetCore.AzureOps.Helpers;
using DurableFunctions.Demo.DotNetCore.AzureOps.Models;
using DurableFunctions.Demo.DotNetCore.AzureOps.Orchestrations.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.Demo.DotNetCore.AzureOps.Orchestrations
{
    public static class OnboardEmployee
    {
        [FunctionName(nameof(OnboardEmployee))]
        public static async Task<OnboardEmployeeOutput> Run(
            [OrchestrationTrigger]DurableOrchestrationContextBase context,
            ILogger logger
            )
        {
            var input = context.GetInput<OnboardEmployeeInput>();
            await context.CallActivityAsync(nameof(ValidateOrchestrationInput), input);

            var getCountryAndRegionInput = Builder.CreateRegionAndCountryCodeInput(input.Location);
            var getCountryAndRegionOutput = 
                await context.CallActivityAsync<GetRegionAndCountryCodeOutput>(
                    nameof(GetRegionAndCountryCode),
                    getCountryAndRegionInput);
            
            Dictionary<string, Environment> resourceGroupNamesAndEnvironments = 
                await GetResourceGroupsAndEnvironments(
                    context,
                    input);

            var createdResources = new List<CreatedResource>();

            createdResources.AddRange( 
                await CreateResourceGroups(
                    context,
                    getCountryAndRegionOutput,
                    resourceGroupNamesAndEnvironments,
                    input.UserName));

            createdResources.AddRange(
                await CreateWebApps(
                    context,
                    getCountryAndRegionOutput,
                    resourceGroupNamesAndEnvironments,
                    input.UserThreeLetterCode,
                    input.UserName));

            var slackInput = new SendSlackNotificationInput
            {
                Message = "Created Azure Resources",
                CreatedResources = createdResources.ToArray(),
                User = input.UserName
            };
            await context.CallActivityAsync(nameof(SendSlackNotification), slackInput);

            return new OnboardEmployeeOutput
            {
                CreatedResources = createdResources.ToArray()
            };
        }

        private static async Task<Dictionary<string, Environment>> GetResourceGroupsAndEnvironments(
            DurableOrchestrationContextBase context,
            OnboardEmployeeInput input)
        {
            var tasks = new List<Task<GetResourceGroupNameOutput>>();
            foreach (var environment in input.Environments)
            {
                var getResourceGroupNameInput = Builder.CreateGetResourceGroupNameInput(
                    environment,
                    input.UserThreeLetterCode);

                tasks.Add(
                    context.CallActivityAsync<GetResourceGroupNameOutput>(
                        nameof(GetResourceGroupName),
                        getResourceGroupNameInput)
                );
            }

            await Task.WhenAll(tasks);

            return tasks.Select(task => new KeyValuePair<string, Environment>(task.Result.ResourceGroup, task.Result.Environment)).ToDictionary(p=> p.Key, p=> p.Value);
        }

        private static async Task<IEnumerable<CreatedResource>> CreateResourceGroups(
            DurableOrchestrationContextBase context,
            GetRegionAndCountryCodeOutput regionAndCountry,
            IDictionary<string, Environment> resourceGroupsAndEnvironments,
            string userName)
        {
            var tasks = new List<Task<CreateResourceGroupOutput>>();
            foreach (var resourceGroupAndEnvironment in resourceGroupsAndEnvironments)
            {
                var createResourceGroupInput = Builder.CreateResourceGroupInput(
                    regionAndCountry,
                    resourceGroupAndEnvironment,
                    userName);
                tasks.Add(context.CallActivityAsync<CreateResourceGroupOutput>(
                    nameof(CreateResourceGroup),
                    createResourceGroupInput)
                );
            }

            await Task.WhenAll(tasks);

            return tasks.Select(task => task.Result.CreatedResource).ToList();
        }

        private static async Task<IEnumerable<CreatedResource>> CreateWebApps(
            DurableOrchestrationContextBase context,
            GetRegionAndCountryCodeOutput regionAndCountry,
            IDictionary<string, Environment> resourceGroupsAndEvEnvironments,
            string userThreeLetterCode,
            string userName)
        {
            var tasks = new List<Task<CreateWebAppOutput>>();
            foreach (var resourceGroupAndEnvironment in resourceGroupsAndEvEnvironments)
            {
                var createWebAppInput = Builder.CreateWebAppInput(
                    regionAndCountry,
                    resourceGroupAndEnvironment,
                    userThreeLetterCode,
                    userName);
                tasks.Add(context.CallActivityAsync<CreateWebAppOutput>(
                    nameof(CreateWebApp),
                    createWebAppInput)
                );
            }

            await Task.WhenAll(tasks);

            return tasks.Select(task => task.Result.CreatedResource).ToList();
        }   
    }
}
