using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;

namespace DurableFunctions.Demo.DotNetCore.Test.AzureOps
{
    public abstract class FunctionTestBase
    {
        protected readonly Fixture Fixture = new Fixture();

        protected ILogger FakeLogger => new Mock<ILogger>().Object;
    }
}
