using System;
using System.Collections.Generic;

namespace Sheaft.Application.Commands
{
    public class SetProductsAvailabilityCommand : Command<bool>
    {
        public SetProductsAvailabilityCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public IEnumerable<Guid> Ids { get; set; }
        public bool Available { get; set; }
    }
}
