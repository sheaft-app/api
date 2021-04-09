using System;
using System.Collections.Generic;

namespace Sheaft.Application.Models
{
    public class CreateAgreementDto
    {
        public Guid StoreId { get; set; }
        public Guid DeliveryModeId { get; set; }
        public Guid? CatalogId { get; set; }
        public IEnumerable<TimeSlotGroupDto> SelectedHours { get; set; }
    }
}