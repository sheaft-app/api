using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Models.ViewModels
{
    public class UserViewModel
    {
        public Guid Id { get; set; }
        public ProfileKind Kind { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Picture { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public string Company { get; set; }
    }
}
