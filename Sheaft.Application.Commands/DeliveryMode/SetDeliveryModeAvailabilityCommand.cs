using System;
using System.Collections.Generic;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class SetDeliveryModeAvailabilityCommand : Command<bool>
    {
        [JsonConstructor]
        public SetDeliveryModeAvailabilityCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public bool Available { get; set; }
    }
}
