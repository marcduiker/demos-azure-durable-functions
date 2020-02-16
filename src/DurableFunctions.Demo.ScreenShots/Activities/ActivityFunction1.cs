using System.Threading.Tasks;
using DurableFunctions.Demo.ScreenShots.Interfaces;
using DurableFunctions.Demo.ScreenShots.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.Demo.ScreenShots.Activities
{
    public class ActivityFunction1
    {
        private readonly IService _service;

        public ActivityFunction1(IService service)
        {
            _service = service;
        }

        [FunctionName(nameof(ActivityFunction1))]
        public async Task<Function1Result> Run(
            [ActivityTrigger] string name,
            ILogger logger)
        {
            var result = await _service.GetDetailsAsync(name);

            return new Function1Result(result);
        }
    }
}