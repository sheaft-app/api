using Sheaft.Domain.Enums;
using System;

namespace Sheaft.Application.Models
{
    public class UserProfileViewModel
    {
        public Guid Id { get; set; }
        public ProfileKind Kind { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
