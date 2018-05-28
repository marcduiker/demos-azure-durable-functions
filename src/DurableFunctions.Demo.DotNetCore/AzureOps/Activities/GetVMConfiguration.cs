using System;
using System.Threading.Tasks;
using DurableFunctions.Demo.DotNetCore.AzureOps.Activities.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace DurableFunctions.Demo.DotNetCore.AzureOps.Activities
{
    public static class GetVMConfiguration
    {
        [FunctionName(nameof(GetVMConfiguration))]
        public static async Task<VMConfiguration> Run(
            [ActivityTrigger] DurableActivityContext activityContext,
            TraceWriter logger)
        {
            var userRole = activityContext.GetInput<string>();

            try
            {
                // Read VM configuration from Table Storage

                return new VMConfiguration();
                
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw;
            }
        }
    }
}
