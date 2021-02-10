using System;

namespace Sheaft.Application.Common.Models.Dto
{
    public class ConsumerDto : UserDto
    {
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public bool Anonymous { get; set; }
    }
}