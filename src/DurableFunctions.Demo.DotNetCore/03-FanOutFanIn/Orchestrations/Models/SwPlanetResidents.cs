// ReSharper disable once CheckNamespace
namespace DurableFunctions.Demo.DotNetCore.FanOutFanIn.Orchestrations.Models
{
    public sealed class SwPlanetResidents
    {
        public string PlanetName { get; set; }

        public string[] Residents { get; set; }
    }
}