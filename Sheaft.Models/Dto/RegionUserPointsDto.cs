using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Models.Dto
{
    public class RegionUserPointsDto
    {
        public Guid UserId { get; set; }
        public ProfileKind Kind { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public Guid RegionId { get; set; }
        public int? Points { get; set; }
        public long Position { get; set; }
    }
}