using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Models.ViewModels
{
    public class TimeSlotViewModel
    {
        public DayOfWeek Day { get; set; }
        public TimeSpan From { get; set; }
        public TimeSpan To { get; set; }
    }
}
