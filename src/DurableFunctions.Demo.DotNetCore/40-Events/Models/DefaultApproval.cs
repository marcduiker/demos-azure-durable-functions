namespace DurableFunctions.Demo.DotNetCore.Models
{
    public class DefaultApproval : Approval
    {
        public DefaultApproval() : base(
            "default approval", 
            false, 
            string.Empty)
        {
        }
    }
}