using System;
using System.Collections.Generic;

namespace Sheaft.Application.Common.Models.Inputs
{
    public class IdTimeSlotGroupInput
    {
        public Guid Id { get; set; }
        public IEnumerable<TimeSlotGroupInput> SelectedHours { get; set; }
    }
}