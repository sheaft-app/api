using System;
using System.Collections.Generic;

namespace Sheaft.Models.Inputs
{
    public class CreateAgreementInput
    {
        public Guid StoreId { get; set; }
        public Guid DeliveryModeId { get; set; }
        public IEnumerable<TimeSlotGroupInput> SelectedHours { get; set; }
    }
}