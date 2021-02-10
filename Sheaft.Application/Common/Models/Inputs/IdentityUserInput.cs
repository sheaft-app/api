using System;
using System.Collections.Generic;

namespace Sheaft.Application.Common.Models.Inputs
{
    public class IdentityUserInput
    {
        public IdentityUserInput(Guid id, string email, string name, string firstname, string lastname, string picture = null, IEnumerable<Guid> roles = null)
        {
            Id = id.ToString("N");
            Email = email;
            Name = name;
            FirstName = firstname;
            LastName = lastname;
            Picture = picture;
            Roles = roles ?? new List<Guid>();
        }

        public string Id { get; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Picture { get; set; }
        public IEnumerable<Guid> Roles { get; set; }
    }
}