using System;

namespace Sheaft.Models.Dto
{
    public class ConsumerDto : UserDto
    {
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Anonymous { get; set; }
    }
}