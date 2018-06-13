using DurableFunctions.Demo.DotNetCore.AzureOps.Orchestrations.Models;
using FluentValidation;

namespace DurableFunctions.Demo.DotNetCore.AzureOps.Validators
{
    public class OnboardEmployeeInputValidator : AbstractValidator<OnboardEmployeeInput>
    {
        public OnboardEmployeeInputValidator()
        {
            RuleFor(input => input.UserName).NotEmpty();
            RuleFor(input => input.Environments).NotEmpty();
            RuleFor(input => input.Location).NotEmpty();
            RuleFor(input => input.Role).NotEmpty();
            RuleFor(input => input.UserThreeLetterCode).Length(3);
        }
    }
}
