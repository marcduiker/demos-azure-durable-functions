using System;
using System.Collections.Generic;

namespace DurableFunctions.Demo.ScreenShots.Models
{
    public class Function1Result
    {
        public Function1Result(IEnumerable<string> details)
        {
            Details = details;
        }

        public Guid Id { get; set; }
        public IEnumerable<string> Details { get; set; }
    }
}