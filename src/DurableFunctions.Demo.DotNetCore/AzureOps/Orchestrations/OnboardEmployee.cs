using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DurableFunctions.Demo.DotNetCore.AzureOps.Activities;
using DurableFunctions.Demo.DotNetCore.AzureOps.Activities.Models;
using DurableFunctions.Demo.DotNetCore.AzureOps.DomainModels;
using DurableFunctions.Demo.DotNetCore.AzureOps.Orchestrations.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace DurableFunctions.Demo.DotNetCore.AzureOps.Orchestrations
{
    public static class OnboardEmployee
    {
        [FunctionName(nameof(OnboardEmployee))]
        public static async Task<OnboardEmployeeOutput> Run(
            [OrchestrationTrigger]DurableOrchestrationContextBase context,
            TraceWriter logger
            )
        {
            var input = context.GetInput<OnboardEmployeeInput>();

            var getCountryAndRegionInput = CreateRegionAndCountryCodeInput(input.Location);
            var getCountryAndRegionOutput = await context.CallActivityAsync<GetRegionAndCountryCodeOutput>(
                nameof(GetRegionAndCountryCode),
                getCountryAndRegionInput);

            var addUserInput = CreateAddUserInput(input, getCountryAndRegionOutput);

            var addUserOutput = await context.CallActivityAsync<AddUserOutput>(
                nameof(AddUser), 
                addUserInput);
            
            IEnumerable<string> resourceGroupNames = await GetResourceGroupNames(
                context,
                input);

            IEnumerable<string> resourceGroupIds = await CreateResourceGroups(
                context,
                getCountryAndRegionOutput,
                resourceGroupNames);
            
            // Create App Service Plan(s) for the Environments
            
            // Notify user

            return new OnboardEmployeeOutput
            {
                Email = addUserOutput.Email,
                ResourceGroupIDs = resourceGroupIds.ToArray()
            };
        }

        private static async Task<IEnumerable<string>> GetResourceGroupNames(
            DurableOrchestrationContextBase context,
            OnboardEmployeeInput input)
        {
            var tasks = new List<Task<GetResourceGroupNameOutput>>();
            foreach (var environment in input.Environments)
            {
                var getResourceGroupNameInput = CreateGetResourceGroupNameInput(
                    environment,
                    input.UserThreeLetterCode);

                tasks.Add(
                    context.CallActivityAsync<GetResourceGroupNameOutput>(
                        nameof(GetResourceGroupName),
                        getResourceGroupNameInput)
                );
            }

            await Task.WhenAll(tasks);

            return tasks.Select(task => task.Result.ResourceGroup).ToList();
        }

        private static async Task<IEnumerable<string>> CreateResourceGroups(
            DurableOrchestrationContextBase context,
            GetRegionAndCountryCodeOutput regionAndCountry,
            IEnumerable<string> resourceGroupNames)
        {
            var tasks = new List<Task<CreateResourceGroupOutput>>();
            foreach (var resourceGroupName in resourceGroupNames)
            {

                var createResourceGroupInput = CreateResourceGroupInput(
                    regionAndCountry,
                    resourceGroupName);
                tasks.Add(context.CallActivityAsync<CreateResourceGroupOutput>(
                    nameof(CreateResourceGroup),
                    createResourceGroupInput)
                );
            }

            await Task.WhenAll(tasks);

            return tasks.Select(task => task.Result.ResourceGroup.Id).ToList();
        }

        private static AddUserInput CreateAddUserInput(
            OnboardEmployeeInput input,
            GetRegionAndCountryCodeOutput regionAndCountry)
        {
            return new AddUserInput
            {
                CountryIsoCode = regionAndCountry.CountryIsoCode,
                UserName = input.UserName
            };
        }

        private static GetRegionAndCountryCodeInput CreateRegionAndCountryCodeInput(string userLocation)
        {
            return new GetRegionAndCountryCodeInput
            {
                UserLocation = userLocation
            };
        }

        private static GetResourceGroupNameInput CreateGetResourceGroupNameInput(
            Environment environment,
            string userThreeLetterCode)
        {
            return new GetResourceGroupNameInput
            {
                Environment = environment,
                UserThreeLetterCode = userThreeLetterCode
            };
        }

        private static CreateResourceGroupInput CreateResourceGroupInput(
            GetRegionAndCountryCodeOutput regionAndCountry,
            string resourceGroupName
            )
        {
            return new CreateResourceGroupInput
            {
                Region = regionAndCountry.Region,
                ResourceGroupName = resourceGroupName
            };
        }
    }
}
