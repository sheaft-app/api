﻿using System;
using System.Collections.Generic;
using Sheaft.Application.Models;
using Sheaft.Domain.Enums;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class UpdateDeliveryModeCommand : Command<bool>
    {
        [JsonConstructor]
        public UpdateDeliveryModeCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public DeliveryKind Kind { get; set; }
        public string Description { get; set; }
        public int LockOrderHoursBeforeDelivery { get; set; }
        public LocationAddressInput Address { get; set; }
        public IEnumerable<TimeSlotGroupInput> OpeningHours { get; set; }
        public bool Available { get; set; }
        public bool AutoAcceptRelatedPurchaseOrder { get; set; }
        public bool AutoCompleteRelatedPurchaseOrder { get; set; }
    }
}
