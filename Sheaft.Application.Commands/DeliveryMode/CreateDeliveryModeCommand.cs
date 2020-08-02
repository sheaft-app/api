﻿using System;
using System.Collections.Generic;
using Sheaft.Models.Inputs;
using Sheaft.Interop.Enums;

namespace Sheaft.Application.Commands
{
    public class CreateDeliveryModeCommand : Command<Guid>
    {
        public CreateDeliveryModeCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public DeliveryKind Kind { get; set; }
        public int LockOrderHoursBeforeDelivery { get; set; }
        public AddressInput Address { get; set; }
        public IEnumerable<TimeSlotGroupInput> OpeningHours { get; set; }
    }
}
