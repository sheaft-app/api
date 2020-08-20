using System;
using System.Collections.Generic;

namespace Sheaft.Application.Commands
{
    public class UnarchiveJobCommand : Command<bool>
    {
        public UnarchiveJobCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
