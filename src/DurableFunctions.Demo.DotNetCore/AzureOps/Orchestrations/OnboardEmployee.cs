using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DurableFunctions.Demo.DotNetCore.AzureOps.Activities;
using DurableFunctions.Demo.DotNetCore.AzureOps.Activities.Models;
using DurableFunctions.Demo.DotNetCore.AzureOps.DomainModels;
using DurableFunctions.Demo.DotNetCore.AzureOps.Models;
using DurableFunctions.Demo.DotNetCore.AzureOps.Orchestrations.Models;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
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

            var getAzureRegionInput = CreateGetAzureRegionInput(input.Location);
            var getAzureRegionOutput = await context.CallActivityAsync<GetAzureRegionOutput>(
                nameof(GetAzureRegion),
                getAzureRegionInput);

            IEnumerable<string> resourceGroupNames = await GetResourceGroupNames(
                context,
                input);

            IEnumerable<string> resourceGroupIds = await CreateResourceGroups(
                context,
                getAzureRegionOutput.Region,
                resourceGroupNames);


            // Check what VM type/size belongs to the role

            // Create VM
            //await context.CallActivityAsync();

            return new OnboardEmployeeOutput {ResourceGroupIDs = resourceGroupIds.ToArray()};
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
            string region,
            IEnumerable<string> resourceGroupNames)
        {
            var tasks = new List<Task<CreateResourceGroupOutput>>();
            foreach (var resourceGroupName in resourceGroupNames)
            {

                var createResourceGroupInput = CreateResourceGroupInput(
                    region,
                    resourceGroupName);
                tasks.Add(context.CallActivityAsync<CreateResourceGroupOutput>(
                    nameof(CreateResourceGroup),
                    createResourceGroupInput)
                );
            }

            await Task.WhenAll(tasks);

            return tasks.Select(task => task.Result.ResourceGroup.Id).ToList();
        }

        private static GetAzureRegionInput CreateGetAzureRegionInput(string userLocation)
        {
            return new GetAzureRegionInput { UserLocation = userLocation };
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
            string region,
            string resourceGroupName
            )
        {
            return new CreateResourceGroupInput
            {
                Region = region,
                ResourceGroupName = resourceGroupName
            };
        }
    }
}
