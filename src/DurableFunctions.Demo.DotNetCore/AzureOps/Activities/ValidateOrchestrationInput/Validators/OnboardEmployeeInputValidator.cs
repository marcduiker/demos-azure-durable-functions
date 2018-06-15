using DurableFunctions.Demo.DotNetCore.AzureOps.Models;
using DurableFunctions.Demo.DotNetCore.AzureOps.Orchestrations.Models;
using FluentValidation;

namespace DurableFunctions.Demo.DotNetCore.AzureOps.Activities.ValidateOrchestrationInput.Validators
{
    public class OnboardEmployeeInputValidator : AbstractValidator<OnboardEmployeeInput>
    {
        public OnboardEmployeeInputValidator()
        {
            DefaultValidatorExtensions.NotEmpty<OnboardEmployeeInput, string>(RuleFor(input => input.UserName));
            DefaultValidatorExtensions.NotEmpty<OnboardEmployeeInput, Environment[]>(RuleFor(input => input.Environments));
            DefaultValidatorExtensions.NotEmpty<OnboardEmployeeInput, string>(RuleFor(input => input.Location));
            DefaultValidatorExtensions.NotEmpty<OnboardEmployeeInput, string>(RuleFor(input => input.Role));
            DefaultValidatorExtensions.Length<OnboardEmployeeInput>(RuleFor(input => input.UserThreeLetterCode), 3);
        }
    }
}
