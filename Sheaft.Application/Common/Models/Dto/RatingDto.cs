using System;

namespace Sheaft.Application.Common.Models.Dto
{
    public class RatingDto
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public decimal Value { get; set; }
        public string Comment { get; set; }
        public UserDto User { get; set; }
    }
}