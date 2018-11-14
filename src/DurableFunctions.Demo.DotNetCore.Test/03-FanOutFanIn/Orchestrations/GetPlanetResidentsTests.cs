using System.Linq;
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
        public void GivenAPlanetIsFoundWith10Residents_WhenGetPlanetResidentsIsCalled_Then10ResidentsShouldBeReturned()
        {
            // Arrange
            const int numberOfResidents = 10;
            var fakeOrchestrationContext = GetFakeOrchestrationContextWithPlanetAndCharacters(numberOfResidents);

            // Act
            var result = GetPlanetResidents.Run(fakeOrchestrationContext, GetFakeLogger());

            // Assert
            result.Result.Residents.Count().Should().Be(10);

        }

        [Fact]
        public void GivenAPlanetIsFoundWithResidents_WhenGetPlanetResidentsIsCalled_ThenResidentsShouldBeReturned()
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
                context => context.CallActivityAsync<Planet>(
                    nameof(SearchPlanet), 
                    It.IsAny<string>()))
                .Returns(Task.FromResult((Planet)null));

            return mock.Object;
        }

        private DurableOrchestrationContextBase GetFakeOrchestrationContextWithPlanetAndCharacters(int numberOfResidents = 3)
        {
            _fixture.RepeatCount = numberOfResidents;
            var mock = new Mock<DurableOrchestrationContextBase>();
            mock.Setup(
                context => context.CallActivityAsync<Planet>(
                    nameof(SearchPlanet), 
                    It.IsAny<string>()))
                .Returns(Task.FromResult(_fixture.Create<Planet>()));

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
