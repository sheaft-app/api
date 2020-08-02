using System;
using System.Collections.Generic;

namespace Sheaft.Application.Commands
{
    public class PauseJobCommand : Command<bool>
    {
        public PauseJobCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
