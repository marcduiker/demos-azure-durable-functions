using System.Threading.Tasks;
using AutoFixture;
using DurableFunctions.Demo.DotNetCore.FanOutFanIn.Activities;
using DurableFunctions.Demo.DotNetCore.FanOutFanIn.Orchestrations;
using DurableFunctions.Demo.DotNetCore.FanOutFanIn.Orchestrations.Models;
using FluentAssertions;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

// ReSharper disable once CheckNamespace
namespace DurableFunctions.Demo.DotNetCore.Test.FanOutFanIn.Orchestrations
{
    public class GetPlanetResidentsTests
    {
        private readonly Fixture _fixture = new Fixture();

        [Fact]
        public void GivenAPlanetIsNotFound_WhenGetPlanetResidentsIsCalled_ThenResidentsShouldBeNull()
        {
            // Arrange
            var fakeOrchestrationContext = GetFakeOrchestrationContextReturnNullForPlanet();

            // Act
            var result = GetPlanetResidents.Run(fakeOrchestrationContext, GetFakeLogger());

            // Assert
            result.Result.Residents.Should().BeNull();

        }

        [Fact]
        public void GivenAPlanetIsFound_WhenGetPlanetResidentsIsCalled_ThenResidentsShouldBeReturned()
        {
            // Arrange
            var fakeOrchestrationContext = GetFakeOrchestrationContextWithPlanetAndCharacters();

            // Act
            var result = GetPlanetResidents.Run(fakeOrchestrationContext, GetFakeLogger());

            // Assert
            result.Result.Residents.Should().NotBeEmpty();

        }

        private static DurableOrchestrationContextBase GetFakeOrchestrationContextReturnNullForPlanet()
        {
            var mock = new Mock<DurableOrchestrationContextBase>();
            mock.Setup(
                context => context.CallActivityAsync<SwPlanet>(
                    nameof(SearchPlanet), 
                    It.IsAny<string>()))
                .Returns(Task.FromResult((SwPlanet)null));

            return mock.Object;
        }

        private DurableOrchestrationContextBase GetFakeOrchestrationContextWithPlanetAndCharacters()
        {
            var mock = new Mock<DurableOrchestrationContextBase>();
            mock.Setup(
                context => context.CallActivityAsync<SwPlanet>(
                    nameof(SearchPlanet), 
                    It.IsAny<string>()))
                .Returns(Task.FromResult(_fixture.Create<SwPlanet>()));

            mock.Setup(
                context => context.CallActivityAsync<string>(
                    nameof(GetCharacter), 
                    It.IsAny<string>()))
                .Returns(Task.FromResult(_fixture.Create<string>()));

            return mock.Object;
        }

        private static ILogger GetFakeLogger()
        {
            return new Mock<ILogger>().Object;
        }
    }
}
