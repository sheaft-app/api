using System;

namespace Sheaft.Web.Manage.Models
{
    public class TimeSlotViewModel
    {
        public DayOfWeek Day { get; set; }
        public TimeSpan From { get; set; }
        public TimeSpan To { get; set; }
    }
}
