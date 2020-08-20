using System;
using System.Collections.Generic;

namespace Sheaft.Application.Commands
{
    public class DeleteJobCommand : Command<bool>
    {
        public DeleteJobCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
