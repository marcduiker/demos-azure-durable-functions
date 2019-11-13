using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

// ReSharper disable once CheckNamespace
namespace DurableFunctions.Demo.DotNetCore.LargeMessages.Activities
{
    public class LargeMessageActivity
    {
        [FunctionName(nameof(LargeMessageActivity))]
        public async Task<string> Run(
            [ActivityTrigger] int nrOfChars,
            ILogger logger)
        {
            return await Task.FromResult(new string('X', nrOfChars));
        }
    }
}
