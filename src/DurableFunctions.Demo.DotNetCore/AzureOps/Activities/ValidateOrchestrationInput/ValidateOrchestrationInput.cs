using System.Linq;
using DurableFunctions.Demo.DotNetCore.AzureOps.Orchestrations.Models;
using DurableFunctions.Demo.DotNetCore.AzureOps.Validators;
using DurableTask.Core.Exceptions;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.Demo.DotNetCore.AzureOps.Activities.ValidateOrchestrationInput
{
    public static class ValidateOrchestrationInput
    {
        [FunctionName(nameof(ValidateOrchestrationInput))]
        public static void Run(
            [ActivityTrigger] OnboardEmployeeInput input,
            ILogger logger)
        {
            var inputValidator = new OnboardEmployeeInputValidator();
            var validationResult = inputValidator.Validate(input);

            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(error => error.ErrorMessage));
                throw new TaskFailedException(message);
            }
        }
    }
}
