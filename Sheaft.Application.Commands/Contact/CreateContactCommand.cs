using System;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class CreateContactCommand : Command<bool>
    {
        public CreateContactCommand(RequestUser user) : base(user)
        {
        }

        public string FirstName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
