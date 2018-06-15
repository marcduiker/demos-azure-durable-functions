using System;
using AutoFixture;
using DurableFunctions.Demo.DotNetCore.AzureOps.Activities.ValidateOrchestrationInput;
using DurableFunctions.Demo.DotNetCore.AzureOps.Orchestrations.Models;
using DurableTask.Core.Exceptions;
using FluentAssertions;
using Xunit;
using Environment = DurableFunctions.Demo.DotNetCore.AzureOps.Models.Environment;

namespace DurableFunctions.Demo.DotNetCore.Test.AzureOps.Activities
{
    public class ValidateOrchestrationInputTests : FunctionTestBase
    {
        [Fact]
        [Trait("Type", "Activity")]
        public void GivenInputHasEmptyProperties_WhenValidateOrchestrationInputIsCalled_TheFunctionShouldThrowTaskFailedException()
        {
            // Arrange
            var input = GetInvalidOnboardEmployeeInputNoProperties();

            // Act
            var action = new Action(()=> ValidateOrchestrationInput.Run(input, FakeLogger));

            // Assert
            action.Should().Throw<TaskFailedException>();
        }

        [Fact]
        [Trait("Type", "Activity")]
        public void GivenUserThreeLetterCodeIsTooShort_WhenValidateOrchestrationInputIsCalled_TheFunctionShouldThrowTaskFailedException()
        {
            // Arrange
            var input = GetInvalidOnboardEmployeeInputWithTwoLetterUsercodeInsteadOfThreeLetters();

            // Act
            var action = new Action(() => ValidateOrchestrationInput.Run(input, FakeLogger));

            // Assert
            action.Should().Throw<TaskFailedException>();
        }

        [Fact]
        [Trait("Type", "Activity")]
        public void GivenValidInput_WhenValidateOrchestrationInputIsCalled_TheFunctionShouldNotThrowAnException()
        {
            // Arrange
            var input = GetValidOnboardEmployeeInput();

            // Act
            var action = new Action(() => ValidateOrchestrationInput.Run(input, FakeLogger));

            // Assert
            action.Should().NotThrow<TaskFailedException>();
        }

        private OnboardEmployeeInput GetValidOnboardEmployeeInput()
        {
            var input = Fixture.Create<OnboardEmployeeInput>();
            input.UserThreeLetterCode = Fixture.Create<string>().Substring(0, 3);

            return input;
        }

        private OnboardEmployeeInput GetInvalidOnboardEmployeeInputWithTwoLetterUsercodeInsteadOfThreeLetters()
        {
            return new OnboardEmployeeInput
            {
                UserName = Fixture.Create<string>(),
                Environments = Fixture.Create<Environment[]>(),
                Location = Fixture.Create<string>(),
                Role = Fixture.Create<string>(),
                UserThreeLetterCode = Fixture.Create<string>().Substring(0, 2)
            };
        }

        private static OnboardEmployeeInput GetInvalidOnboardEmployeeInputNoProperties()
        {
            return new OnboardEmployeeInput();
        }
    }
}
