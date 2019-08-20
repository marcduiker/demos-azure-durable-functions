using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace DurableFunctions.Demo.DotNetCore.Reflection
{
    public class FunctionsOverview
    {
        public IEnumerable<string> ClientFunctions { get; set; }

        public IEnumerable<string> OrchestratorFunctions { get; set; }

        public IEnumerable<string> ActivityFunctions { get; set; }
    }
}