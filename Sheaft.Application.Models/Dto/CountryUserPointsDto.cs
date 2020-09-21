using Sheaft.Domain.Enums;
using System;

namespace Sheaft.Application.Models
{
    public class CountryUserPointsDto
    {
        public Guid UserId { get; set; }
        public ProfileKind Kind { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public int? Points { get; set; }
        public long Position { get; set; }
    }
}