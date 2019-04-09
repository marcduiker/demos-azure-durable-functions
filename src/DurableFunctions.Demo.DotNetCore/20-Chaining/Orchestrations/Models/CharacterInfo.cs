// ReSharper disable once CheckNamespace
namespace DurableFunctions.Demo.DotNetCore.Chaining.Orchestrations.Models
{
    public sealed class CharacterInfo
    {
        public string Name { get; set; }

        public string HomeWorld { get; set; }

        public string Species { get; set; }

        public string SpeciesHomeWorld { get; set; }
    }
}