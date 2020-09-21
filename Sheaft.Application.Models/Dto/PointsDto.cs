using Sheaft.Domain.Enums;
using System;

namespace Sheaft.Application.Models
{
    public class PointsDto
    {
        public DateTimeOffset CreatedOn { get; set; }
        public PointKind Kind { get; set; }
        public int Quantity { get; set; }
    }
}