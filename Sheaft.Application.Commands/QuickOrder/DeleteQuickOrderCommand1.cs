using System;
using System.Collections.Generic;

namespace Sheaft.Application.Commands
{
    public class DeleteQuickOrderCommand : Command<bool>
    {
        public DeleteQuickOrderCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
