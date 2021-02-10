using System;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Common.Models.Dto
{
    public class PointsDto
    {
        public DateTimeOffset CreatedOn { get; set; }
        public PointKind Kind { get; set; }
        public int Quantity { get; set; }
    }
}