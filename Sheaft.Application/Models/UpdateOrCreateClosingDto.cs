using System;

namespace Sheaft.Application.Models
{
    public class UpdateOrCreateClosingDto
    {
        public Guid? Id { get; set; }
        public DateTimeOffset From { get; set; }
        public DateTimeOffset To { get; set; }
        public string Reason { get; set; }
    }
}