using Microsoft.Azure.WebJobs;
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
