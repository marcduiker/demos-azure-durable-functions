using System.Runtime.InteropServices.WindowsRuntime;

namespace DurableFunctions.Demo.DotNetCore.Models
{
    public static class ApprovalBuilder
    {
        public static Approval BuildDefault()
        {
            return new DefaultApproval();
            ;
        }
    }
}