﻿using System;

namespace Sheaft.Models.Dto
{
    public class TimeSlotDto
    {
        public DayOfWeek Day { get; set; }
        public TimeSpan From { get; set; }
        public TimeSpan To { get; set; }
    }
}