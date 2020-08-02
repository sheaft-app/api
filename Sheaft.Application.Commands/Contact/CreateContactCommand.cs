using System;

namespace Sheaft.Application.Commands
{
    public class CreateContactCommand : Command<bool>
    {
        public CreateContactCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public string FirstName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
