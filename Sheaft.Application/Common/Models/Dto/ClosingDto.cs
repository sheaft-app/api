using System;

namespace Sheaft.Application.Common.Models.Dto
{
    public class ClosingDto
    {
        public Guid Id { get; set; }
        public DateTimeOffset From { get; set; }
        public DateTimeOffset To { get; set; }
        public string Reason { get; set; }
    }
}