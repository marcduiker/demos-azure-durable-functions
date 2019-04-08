using System;
using System.Threading;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

// ReSharper disable once CheckNamespace
namespace DurableFunctions.Demo.DotNetCore.Basics.Activities
{
    public class SleepingActivity
    {
        [FunctionName(nameof(SleepingActivity))]
        public string Run(
            [ActivityTrigger] int sleepTimeSeconds,
            ILogger logger)
        {

            Thread.Sleep(TimeSpan.FromSeconds(sleepTimeSeconds));

            return $"Waking up after {sleepTimeSeconds} seconds!";
        }
    }
}
