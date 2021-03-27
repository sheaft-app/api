﻿using System;
using System.Collections.Generic;
using Sheaft.Domain.Enum;

namespace Sheaft.Web.Manage.Models
{
    public class DeliveryViewModel
    {
        public Guid Id { get; set; }
        public DeliveryKind Kind { get; set; }
        public string Name { get; set; }
        public AddressViewModel Address { get; set; }
        public IEnumerable<TimeSlotViewModel> DeliveryHours { get; set; }
    }
}