using System;
using System.Collections.Generic;

namespace Sheaft.Models.Inputs
{
    public class IdentityUserInput : UserInput
    {
        public IdentityUserInput(Guid id, string email, string firstname, string lastname, IEnumerable<Guid> roles = null)
        {
            Id = id.ToString("N");
            Email = email;
            FirstName = firstname;
            LastName = lastname;
            Roles = roles ?? new List<Guid>();
        }

        public string Id { get; }
        public IEnumerable<Guid> Roles { get; }
    }
}