using System;

namespace Sheaft.Application.Models
{
    public class ExportPurchaseOrdersDto
    {
        public DateTimeOffset From { get; set; }
        public DateTimeOffset To { get; set; }
        public string Name { get; set; }
    }
}