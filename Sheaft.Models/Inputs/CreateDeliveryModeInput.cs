using Sheaft.Interop.Enums;
using System.Collections.Generic;

namespace Sheaft.Models.Inputs
{
    public class CreateDeliveryModeInput
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DeliveryKind Kind { get; set; }
        public int? LockOrderHoursBeforeDelivery { get; set; }
        public LocationAddressInput Address { get; set; }
        public IEnumerable<TimeSlotGroupInput> OpeningHours { get; set; }
    }
}