using Sheaft.Domain.Enums;
using System;

namespace Sheaft.Application.Models
{
    public class UserViewModel
    {
        public Guid Id { get; set; }
        public ProfileKind Kind { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Anonymous { get; set; }
        public Guid? DepartmentId { get; set; }
        public string Phone { get; set; }
        public string Picture { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public DateTimeOffset? RemovedOn { get; set; }
    }
}
