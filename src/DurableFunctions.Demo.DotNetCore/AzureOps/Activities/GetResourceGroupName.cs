using DurableFunctions.Demo.DotNetCore.AzureOps.Activities.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace DurableFunctions.Demo.DotNetCore.AzureOps.Activities
{
    public static class GetResourceGroupName
    {
        [FunctionName(nameof(GetResourceGroupName))]
        public static GetResourceGroupNameOutput Run(
            [ActivityTrigger] DurableActivityContext activityContext,
            TraceWriter logger)
        {
            var input = activityContext.GetInput<GetResourceGroupNameInput>();
            
            var result = new GetResourceGroupNameOutput
            {
                ResourceGroup = $"megacorp-user-{input.UserThreeLetterCode}-{input.Environment}-rg".ToLower()
            };

            return result;
        }
    }
}
