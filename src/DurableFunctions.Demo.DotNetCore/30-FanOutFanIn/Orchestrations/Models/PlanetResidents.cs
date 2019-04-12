// ReSharper disable once CheckNamespace
namespace DurableFunctions.Demo.DotNetCore.FanOutFanIn.Orchestrators.Models
{
    public sealed class PlanetResidents
    {
        public string PlanetName { get; set; }

        public string[] Residents { get; set; }
    }
}