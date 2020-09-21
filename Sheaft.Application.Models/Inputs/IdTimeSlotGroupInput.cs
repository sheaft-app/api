using System;
using System.Collections.Generic;

namespace Sheaft.Application.Models
{
    public class IdTimeSlotGroupInput
    {
        public Guid Id { get; set; }
        public IEnumerable<TimeSlotGroupInput> SelectedHours { get; set; }
    }
}