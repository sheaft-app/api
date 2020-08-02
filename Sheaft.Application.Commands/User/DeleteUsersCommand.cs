using System;
using System.Collections.Generic;

namespace Sheaft.Application.Commands
{
    public class DeleteUsersCommand : Command<bool>
    {
        public DeleteUsersCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public IEnumerable<Guid> Ids { get; set; }
        public string Reason { get; set; }
    }
}
