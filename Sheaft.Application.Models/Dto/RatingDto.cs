using System;

namespace Sheaft.Application.Models
{
    public class RatingDto
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public decimal Value { get; set; }
        public string Comment { get; set; }
        public UserProfileDto User { get; set; }
    }
}