using System;
using System.Collections.Generic;
using Sheaft.Models.Inputs;
using Sheaft.Interop.Enums;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class CreateDeliveryModeCommand : Command<Guid>
    {
        [JsonConstructor]
        public CreateDeliveryModeCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public DeliveryKind Kind { get; set; }
        public int LockOrderHoursBeforeDelivery { get; set; }
        public LocationAddressInput Address { get; set; }
        public IEnumerable<TimeSlotGroupInput> OpeningHours { get; set; }
    }
}
