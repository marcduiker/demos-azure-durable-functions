using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using System;
using System.Collections.Generic;

namespace DurableFunctions.Demo.DotNetCore.Status
{
    public class GetStatusRequest
    {
        public GetStatusRequest()
        {
            StatussesToMatch = new List<OrchestrationRuntimeStatus>();
        }

        public DateTime? CreatedFrom { get; set; }

        public DateTime? CreatedTo { get; set; }

        public List<OrchestrationRuntimeStatus> StatussesToMatch { get; set; }
    }
}
