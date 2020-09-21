using Sheaft.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Sheaft.Application.Models
{
    public class DeliveryModeViewModel
    {
        public Guid Id { get; set; }
        public DeliveryKind Kind { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public DateTimeOffset? RemovedOn { get; set; }
        public int LockOrderHoursBeforeDelivery { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public AddressViewModel Address { get; set; }
        public BusinessViewModel Producer { get; set; }
        public IEnumerable<TimeSlotViewModel> OpeningHours { get; set; }
    }
}
