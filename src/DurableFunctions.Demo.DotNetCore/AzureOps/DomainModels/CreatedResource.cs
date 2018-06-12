namespace DurableFunctions.Demo.DotNetCore.AzureOps.DomainModels
{
    public class CreatedResource
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string ResourceGroup { get; set; }

        public Environment Environment { get; set; }

        public string Region { get; set; }
    }
}