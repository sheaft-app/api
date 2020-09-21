using Sheaft.Domain.Enums;
using System;

namespace Sheaft.Application.Models
{
    public class TimeSlotViewModel
    {
        public DayOfWeek Day { get; set; }
        public TimeSpan From { get; set; }
        public TimeSpan To { get; set; }
    }
}
