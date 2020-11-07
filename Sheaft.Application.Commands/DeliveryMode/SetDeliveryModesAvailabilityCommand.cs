using System;
using System.Collections.Generic;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class SetDeliveryModesAvailabilityCommand : Command<bool>
    {
        [JsonConstructor]
        public SetDeliveryModesAvailabilityCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public IEnumerable<Guid> Ids { get; set; }
        public bool Available { get; set; }
    }
}
