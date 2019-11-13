using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

// ReSharper disable once CheckNamespace
namespace DurableFunctions.Demo.DotNetCore.Basics.Activities
{
    public class HelloNameActivity
    {
        [FunctionName(nameof(HelloNameActivity))]
        public string Run(
            [ActivityTrigger] string name,
            ILogger logger)
        {
            return $"Hello {name}!";
        }
    }
}
