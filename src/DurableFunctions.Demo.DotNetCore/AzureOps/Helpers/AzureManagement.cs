using System;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;

namespace DurableFunctions.Demo.DotNetCore.AzureOps.Helpers
{
    public class AzureManagement
    {
        private static AzureManagement _azureManagement;
        private readonly IAzure _azure;

        public IAzure AzureApi => _azure;

        static AzureManagement()
        {
        }

        private AzureManagement()
        {
            string applicationId = Environment.GetEnvironmentVariable("AzureOpsAppServicePrincipalApplicationId");
            string key = Environment.GetEnvironmentVariable("AzureOpsKey1Value");
            string tenantId = Environment.GetEnvironmentVariable("TenantId");
            string subscriptionId = Environment.GetEnvironmentVariable("SubscriptionId");
            AzureCredentials credentials =
                new AzureCredentialsFactory().FromServicePrincipal(applicationId, key, tenantId,
                    AzureEnvironment.AzureGlobalCloud);
            _azure = Azure.Authenticate(credentials).WithSubscription(subscriptionId);
        }

        public static AzureManagement Instance
        {
            get { return _azureManagement = _azureManagement ?? new AzureManagement(); }

        }
    }
}
