using System;
using System.Collections.Generic;
using DurableFunctions.Demo.DotNetCore.AzureOps.DomainModels;

namespace DurableFunctions.Demo.DotNetCore.AzureOps.Activities.Models
{
    public sealed class SendSlackNotificationInput
    {
        public IList<CreatedResource> CreatedResources { get; set; }

        public string User { get; set; }

        public string Message { get; set; }
    }
}
