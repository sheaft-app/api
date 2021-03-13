using System;

namespace Sheaft.Application.Common.Models.Inputs
{
    public class ClosingInput
    {
        public DateTimeOffset From { get; set; }
        public DateTimeOffset To { get; set; }
        public string Reason { get; set; }
    }
}