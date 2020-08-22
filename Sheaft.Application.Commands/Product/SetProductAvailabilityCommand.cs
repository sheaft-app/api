using System;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class SetProductAvailabilityCommand : Command<bool>
    {
        public SetProductAvailabilityCommand(RequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
        public bool Available { get; set; }
    }
}
