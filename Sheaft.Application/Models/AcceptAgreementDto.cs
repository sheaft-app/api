using System;
using System.Collections.Generic;

namespace Sheaft.Application.Models
{
    public class AcceptAgreementDto
    {
        public Guid Id { get; set; }
        public Guid? CatalogId { get; set; }
        public IEnumerable<TimeSlotGroupDto> SelectedHours { get; set; }
    }
}