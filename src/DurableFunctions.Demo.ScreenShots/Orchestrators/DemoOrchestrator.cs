using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DurableFunctions.Demo.ScreenShots.Activities;
using DurableFunctions.Demo.ScreenShots.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.Demo.ScreenShots.Orchestrators
{
    public class DemoOrchestrator
    {
        [FunctionName(nameof(RunWithChaining))]
        public async Task<DemoOrchestratorResult> RunWithChaining(
            [OrchestrationTrigger]IDurableOrchestrationContext context,
            ILogger log)
        {
            var name = context.GetInput<string>();
            var function1Result = await context.CallActivityAsync<Function1Result>(
                nameof(ActivityFunction1),
                name);
        
            var function2Result = await context.CallActivityAsync<Function2Result>(
                nameof(ActivityFunction2),
                function1Result);
        
            var orchestratorResult = new DemoOrchestratorResult(function1Result, function2Result);
            return orchestratorResult;
        }
        
        [FunctionName(nameof(RunWithChainingAndRetries))]
        public async Task<DemoOrchestratorResult> RunWithChainingAndRetries(
            [OrchestrationTrigger]IDurableOrchestrationContext context,
            ILogger log)
        {
            var name = context.GetInput<string>();
            var function1Result = await context.CallActivityAsync<Function1Result>(
                nameof(ActivityFunction1),
                name);
        
            var function2Result = await context.CallActivityWithRetryAsync<Function2Result>(
                nameof(ActivityFunction2),
                GetDefaultRetryOptions(),
                function1Result);
        
            var orchestratorResult = new DemoOrchestratorResult(function1Result, function2Result);
            return orchestratorResult;
        }
        
        private static RetryOptions GetDefaultRetryOptions()
        {
            return new RetryOptions(TimeSpan.FromSeconds(30), 3) { BackoffCoefficient = 2 };
        }

        

        [FunctionName(nameof(RunWithFanOutFanIn))]
        public async Task RunWithFanOutFanIn(
            [OrchestrationTrigger]IDurableOrchestrationContext context,
            ILogger log)
        {
            var names = context.GetInput<IEnumerable<string>>();
            var taskList = new List<Task<Function1Result>>();
            foreach (var name in names)
            {
                var task = context.CallActivityAsync<Function1Result>(
                    nameof(ActivityFunction1),
                    name);
                taskList.Add(task);
            }

            // Wait for all tasks to complete
            var function1Result = await Task.WhenAll(taskList);
            // Wait for one task that is completed
            //var function1Result = await Task.WhenAny(taskList);
            
            await context.CallActivityAsync<Function2Result>(
                nameof(ActivityFunction2),
                function1Result);
        }
        
        [FunctionName(nameof(RunWithSubOrchestrators))]
        public async Task RunWithSubOrchestrators(
            [OrchestrationTrigger]IDurableOrchestrationContext context,
            ILogger log)
        {
            var name = context.GetInput<string>();
            var function1Result = await context.CallActivityWithRetryAsync<Function1Result>(
                nameof(ActivityFunction1),
                GetDefaultRetryOptions(),
                name);
        
            var orchestratorAResult = await context.CallSubOrchestratorWithRetryAsync<DemoOrchestratorAResult>(
                nameof(DemoOrchestratorB),
                GetDefaultRetryOptions(),
                function1Result);
            
            await context.CallSubOrchestratorWithRetryAsync(
                nameof(DemoOrchestratorB),
                GetDefaultRetryOptions(),
                orchestratorAResult);
        }

        
    }
}
