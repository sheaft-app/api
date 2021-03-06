﻿using System;

namespace Sheaft.Web.Manage.Models
{
    public class TimeSlotViewModel
    {
        public Guid? Id { get; set; }
        public DayOfWeek? Day { get; set; }
        public TimeSpan? From { get; set; }
        public TimeSpan? To { get; set; }
        public bool Remove { get; set; }
    }
}
