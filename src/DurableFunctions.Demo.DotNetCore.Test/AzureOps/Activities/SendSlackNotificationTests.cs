using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using DurableFunctions.Demo.DotNetCore.AzureOps.Activities.Models;
using DurableFunctions.Demo.DotNetCore.AzureOps.Activities.SendSlackNotification;
using Moq;
using Moq.Protected;
using Xunit;

namespace DurableFunctions.Demo.DotNetCore.Test.AzureOps.Activities
{
    public class SendSlackNotificationTests : FunctionTestBase
    {
        [Fact]
        public void GivenValidInput_WhenSendSlackNotificationIsCalled_ThenSendAsyncIsCalledOnTheHttpHandler()
        {
            // Arrange
            var input = GetSlackNotificationInput();
            var httpHandler = GetFakeHttpMessageHandler();
            Environment.SetEnvironmentVariable(SendSlackNotification.EnvironmentVariableSlackWebHook, "http://fake");
            SendSlackNotification.HttpClient = new HttpClient(httpHandler.Object);

            // Act
            SendSlackNotification.Run(input, FakeLogger).Wait();

            // Assert
            httpHandler.Protected().Verify("SendAsync", 
                Times.Once(), 
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>());
        }

        private static Mock<HttpMessageHandler> GetFakeHttpMessageHandler()
        {
            var mockRepsonseMessage = new Mock<HttpResponseMessage>(HttpStatusCode.OK).Object;
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(mockRepsonseMessage);

            return mockHttpMessageHandler;
        }

        private SendSlackNotificationInput GetSlackNotificationInput()
        {
            return Fixture.Create<SendSlackNotificationInput>();
        }
    }
}
