using System;
using Newtonsoft.Json;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class SetProductAvailabilityCommand : Command<bool>
    {
        [JsonConstructor]
        public SetProductAvailabilityCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public bool Available { get; set; }
    }
}
