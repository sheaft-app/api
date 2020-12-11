using Sheaft.Domain.Enums;
using System.Collections.Generic;

namespace Sheaft.Application.Models
{
    public class CreateDeliveryModeInput
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DeliveryKind Kind { get; set; }
        public bool Available { get; set; }
        public bool AutoAcceptRelatedPurchaseOrder { get; set; }
        public bool AutoCompleteRelatedPurchaseOrder { get; set; }
        public int? LockOrderHoursBeforeDelivery { get; set; }
        public LocationAddressInput Address { get; set; }
        public IEnumerable<TimeSlotGroupInput> OpeningHours { get; set; }
    }
}