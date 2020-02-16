using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using System.Reflection;

// ReSharper disable once CheckNamespace
namespace DurableFunctions.Demo.DotNetCore.Reflection
{
    public class GetFunctionMethods
    {
        [FunctionName("GetFunctionMethods")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req)
        {
            var functionsOverview = FunctionNameExtractor.GetFunctionsOverview(Assembly.GetExecutingAssembly());
            
            return new OkObjectResult(functionsOverview);
        }
    }
}
