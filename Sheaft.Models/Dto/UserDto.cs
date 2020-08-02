using System;

namespace Sheaft.Models.Dto
{
    public class UserDto : UserProfileDto
    {
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public DepartmentDto Department { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Anonymous { get; set; }
    }
}