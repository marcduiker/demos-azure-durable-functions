using System;
using System.Threading.Tasks;
using DurableFunctions.Demo.DotNetCore.AzureOps.Activities.Models;
using DurableFunctions.Demo.DotNetCore.AzureOps.Helpers;
using Microsoft.Azure.Management.Compute.Fluent;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace DurableFunctions.Demo.DotNetCore.AzureOps.Activities
{
    public static class CreateCompute
    {
        [FunctionName(nameof(CreateCompute))]
        public static async Task<string> Run(
            [ActivityTrigger] DurableActivityContext activityContext,
            TraceWriter logger)
        {
            var input = activityContext.GetInput<CreateComputeInput>();

            try
            {
                throw new NotImplementedException();
            //    var resourceGroupToCreate = await AzureManagement.Instance.VirtualMachines
            //        .Define(input.VMName)
            //        .WithRegion(input.Region)
            //        .WithExistingResourceGroup(input.ResourceGroup)
            //        .WithExistingPrimaryNetwork()
            //        .WithSubnet("")
            //        .WithPrimaryPrivateIPAddressDynamic()
            //        .WithNewPrimaryPublicIPAddress("")
            //        .WithPopularWindowsImage(KnownWindowsVirtualMachineImage.WindowsServer2012R2Datacenter)
            //        .WithAdminUsername("username")
            //        .WithAdminPassword("pw")

            //        .CreateAsync();

            //    return resourceGroupToCreate.Inner;
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw;
            }
        }
    }
}
