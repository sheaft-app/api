using System;
using System.Collections.Generic;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{

    public class ChangeUserRolesCommand : Command<bool>
    {
        public ChangeUserRolesCommand(RequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
