using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Models.Dto
{
    public class UserProfileDto
    {
        public Guid Id { get; set; }
        public ProfileKind Kind { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Initials { get; set; }
        public string Phone { get; set; }
        public string Picture { get; set; }
    }
}