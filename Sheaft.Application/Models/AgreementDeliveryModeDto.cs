using System;
using System.Collections;
using System.Collections.Generic;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Models
{
    public class AgreementDeliveryModeDto
    {
        public Guid Id { get; set; }
        public DeliveryKind Kind { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public int LockOrderHoursBeforeDelivery { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public AddressDto Address { get; set; }
        public IEnumerable<TimeSlotDto> OpeningHours { get; set; }
    }
}