namespace DurableFunctions.Demo.DotNetCore.Models
{
    public class Approval
    {
        public Approval(string name, bool result, string reason)
        {
            Name = name;
            Result = result;
            Reason = reason;
        }
        
        public string Name { get; set; }

        public bool Result { get; set; }

        public string Reason { get; set; }
    }
}