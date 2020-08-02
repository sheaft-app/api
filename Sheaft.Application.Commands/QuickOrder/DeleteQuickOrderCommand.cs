using System;
using System.Collections.Generic;

namespace Sheaft.Application.Commands
{
    public class DeleteQuickOrdersCommand : Command<bool>
    {
        public DeleteQuickOrdersCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public IEnumerable<Guid> Ids { get; set; }
    }
}
