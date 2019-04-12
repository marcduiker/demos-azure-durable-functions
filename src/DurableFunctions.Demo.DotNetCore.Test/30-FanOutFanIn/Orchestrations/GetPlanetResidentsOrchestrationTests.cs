using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using DurableFunctions.Demo.DotNetCore.FanOutFanIn.Activities;
using DurableFunctions.Demo.DotNetCore.FanOutFanIn.Orchestrators;
using DurableFunctions.Demo.DotNetCore.FanOutFanIn.Orchestrators.Models;
using FluentAssertions;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

// ReSharper disable once CheckNamespace
namespace DurableFunctions.Demo.DotNetCore.Test.FanOutFanIn.Orchestrators
{
    public class GetPlanetResidentsOrchestratorTests
    {
        private readonly Fixture _fixture = new Fixture();

        [Fact]
        public void GivenAPlanetIsNotFound_WhenGetPlanetResidentsIsCalled_ThenResidentsShouldBeNull()
        {
            // Arrange
            var fakeOrchestratorContext = GetFakeOrchestratorContextReturnNullForPlanet();
            var getPlanetResidentsOrchestrator = new GetPlanetResidentsOrchestrator();

            // Act
            var result = getPlanetResidentsOrchestrator.Run(fakeOrchestratorContext, GetFakeLogger());

            // Assert
            result.Result.Residents.Should().BeNull();

        }

        [Fact]
        public void GivenAPlanetIsFoundWith10Residents_WhenGetPlanetResidentsIsCalled_Then10ResidentsShouldBeReturned()
        {
            // Arrange
            const int numberOfResidents = 10;
            var fakeOrchestratorContext = GetFakeOrchestratorContextWithPlanetAndCharacters(numberOfResidents);
            var getPlanetResidentsOrchestrator = new GetPlanetResidentsOrchestrator();

            // Act
            var result = getPlanetResidentsOrchestrator.Run(fakeOrchestratorContext, GetFakeLogger());

            // Assert
            result.Result.Residents.Count().Should().Be(10);

        }

        [Fact]
        public void GivenAPlanetIsFoundWithResidents_WhenGetPlanetResidentsIsCalled_ThenResidentsShouldBeReturned()
        {
            // Arrange
            var fakeOrchestratorContext = GetFakeOrchestratorContextWithPlanetAndCharacters();
            var getPlanetResidentsOrchestrator = new GetPlanetResidentsOrchestrator();
            
            // Act
            var result = getPlanetResidentsOrchestrator.Run(fakeOrchestratorContext, GetFakeLogger());

            // Assert
            result.Result.Residents.Should().NotBeEmpty();

        }

        private static DurableOrchestrationContextBase GetFakeOrchestratorContextReturnNullForPlanet()
        {
            var mock = new Mock<DurableOrchestrationContextBase>(MockBehavior.Strict);
            mock.Setup(
                context => context.GetInput<string>())
                .Returns("tatooine");

            mock.Setup(
                context => context.CallActivityAsync<Planet>(
                    nameof(SearchPlanetActivity), 
                    It.IsAny<string>()))
                .Returns(Task.FromResult((Planet)null));

            return mock.Object;
        }

        private DurableOrchestrationContextBase GetFakeOrchestratorContextWithPlanetAndCharacters(int numberOfResidents = 3)
        {
            _fixture.RepeatCount = numberOfResidents;
            var mock = new Mock<DurableOrchestrationContextBase>(MockBehavior.Strict);
            mock.Setup(
                context => context.GetInput<string>())
                .Returns("tatooine");

            mock.Setup(
                context => context.CallActivityAsync<Planet>(
                    nameof(SearchPlanetActivity), 
                    It.IsAny<string>()))
                .Returns(Task.FromResult(_fixture.Create<Planet>()));

            mock.Setup(
                context => context.CallActivityAsync<string>(
                    nameof(GetCharacterActivity), 
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
