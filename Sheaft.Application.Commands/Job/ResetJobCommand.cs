using System;
using System.Collections.Generic;

namespace Sheaft.Application.Commands
{
    public class ResetJobCommand : Command<bool>
    {
        public ResetJobCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
