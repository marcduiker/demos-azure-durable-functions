using System.Net.Http;
using System.Threading.Tasks;
using DurableFunctions.Demo.DotNetCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.Demo.DotNetCore.Clients
{
    public class RaiseApprovalEventClient
    {
        [FunctionName(nameof(RaiseApprovalEventClient))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]
            HttpRequestMessage req, 
            ILogger log)
        {

            var approval = await req.Content.ReadAsAsync<Approval>();
            
            return new AcceptedResult();
        }
    }
}