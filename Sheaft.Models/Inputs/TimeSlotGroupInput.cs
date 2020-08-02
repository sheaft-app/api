using System;
using System.Collections.Generic;

namespace Sheaft.Models.Inputs
{
    public class TimeSlotGroupInput
    {
        public IEnumerable<DayOfWeek> Days { get; set; }
        public TimeSpan From { get; set; }
        public TimeSpan To { get; set; }
    }
}