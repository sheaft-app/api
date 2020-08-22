using System;
using System.Collections.Generic;
using Sheaft.Models.Inputs;
using Sheaft.Interop.Enums;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class UpdateDeliveryModeCommand : Command<bool>
    {
        public UpdateDeliveryModeCommand(RequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public DeliveryKind Kind { get; set; }
        public string Description { get; set; }
        public int LockOrderHoursBeforeDelivery { get; set; }
        public AddressInput Address { get; set; }
        public IEnumerable<TimeSlotGroupInput> OpeningHours { get; set; }
    }
}
