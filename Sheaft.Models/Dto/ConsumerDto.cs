using System;

namespace Sheaft.Models.Dto
{
    public class ConsumerDto : UserDto
    {
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public bool Anonymous { get; set; }
    }
}