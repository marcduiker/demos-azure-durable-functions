namespace DurableFunctions.Demo.DotNetCore.Models
{
    public class Approval
    {
        public Approval(string name, bool isApproved, string reason)
        {
            Name = name;
            IsApproved = isApproved;
            Reason = reason;
        }
        
        public string Name { get; set; }

        public bool IsApproved { get; set; }

        public string Reason { get; set; }
    }
}