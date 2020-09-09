using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Models.Dto
{
    public class PointsDto
    {
        public DateTimeOffset CreatedOn { get; set; }
        public PointKind Kind { get; set; }
        public int Quantity { get; set; }
    }
}