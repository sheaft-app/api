using System;

namespace Sheaft.Application.Models
{
    public class CreateClosingDto
    {
        public DateTimeOffset From { get; set; }
        public DateTimeOffset To { get; set; }
        public string Reason { get; set; }
    }
}