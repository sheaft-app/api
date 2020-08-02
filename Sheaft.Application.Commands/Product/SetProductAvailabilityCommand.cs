using System;
using System.Collections.Generic;

namespace Sheaft.Application.Commands
{
    public class SetProductAvailabilityCommand : Command<bool>
    {
        public SetProductAvailabilityCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
        public bool Available { get; set; }
    }
}
