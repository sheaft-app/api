using System;
using System.Collections.Generic;

namespace Sheaft.Models.Inputs
{
    public class IdentityUserInput 
    {
        public IdentityUserInput(Guid id, string email, string name, string firstname, string lastname, IEnumerable<Guid> roles = null)
        {
            Id = id.ToString("N");
            Email = email;
            Name = name;
            FirstName = firstname;
            LastName = lastname;
            Roles = roles ?? new List<Guid>();
        }

        public string Id { get; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Picture { get; set; }
        public IEnumerable<Guid> Roles { get; }
    }
}