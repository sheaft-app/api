using System;
using System.Collections.Generic;

namespace Sheaft.Application.Models
{
    public class ResourceIdTimeSlotsDto
    {
        public Guid Id { get; set; }
        public IEnumerable<TimeSlotGroupDto> SelectedHours { get; set; }
    }
}