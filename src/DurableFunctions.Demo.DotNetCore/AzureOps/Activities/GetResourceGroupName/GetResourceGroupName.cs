using DurableFunctions.Demo.DotNetCore.AzureOps.Activities.GetResourceGroupName.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.Demo.DotNetCore.AzureOps.Activities.GetResourceGroupName
{
    public static class GetResourceGroupName
    {
        [FunctionName(nameof(GetResourceGroupName))]
        public static GetResourceGroupNameOutput Run(
            [ActivityTrigger] GetResourceGroupNameInput input,
            ILogger logger)
        {
            var result = new GetResourceGroupNameOutput
            {
                ResourceGroup = $"megacorp-user-{input.UserThreeLetterCode}-{input.Environment}-rg".ToLower()
            };

            return result;
        }
    }
}
