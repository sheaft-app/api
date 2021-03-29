using System.Collections.Generic;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Models
{
    public class CreateDeliveryModeDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DeliveryKind Kind { get; set; }
        public int? MaxPurchaseOrdersPerTimeSlot { get; set; }
        public bool Available { get; set; }
        public bool AutoAcceptRelatedPurchaseOrder { get; set; }
        public bool AutoCompleteRelatedPurchaseOrder { get; set; }
        public int? LockOrderHoursBeforeDelivery { get; set; }
        public AddressDto Address { get; set; }
        public IEnumerable<TimeSlotGroupDto> OpeningHours { get; set; }
        public IEnumerable<UpdateOrCreateClosingDto> Closings { get; set; }
    }
}