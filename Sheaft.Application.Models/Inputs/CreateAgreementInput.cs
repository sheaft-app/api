using System;
using System.Collections.Generic;

namespace Sheaft.Application.Models
{
    public class CreateAgreementInput
    {
        public Guid StoreId { get; set; }
        public Guid DeliveryModeId { get; set; }
        public IEnumerable<TimeSlotGroupInput> SelectedHours { get; set; }
    }
}