using System;
using System.Collections.Generic;

namespace Sheaft.Application.Commands
{

    public class ChangeUserRolesCommand : Command<bool>
    {
        public ChangeUserRolesCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
