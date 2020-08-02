using System;
using System.Collections.Generic;

namespace Sheaft.Application.Commands
{
    public class StartJobCommand : Command<bool>
    {
        public StartJobCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
