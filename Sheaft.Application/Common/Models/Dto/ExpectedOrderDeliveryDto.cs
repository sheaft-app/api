﻿using System;

namespace Sheaft.Application.Common.Models.Dto
{
    public class ExpectedOrderDeliveryDto
    {
        public DateTimeOffset ExpectedDeliveryDate { get; set; }
        public DayOfWeek Day { get; set; }
        public TimeSpan From { get; set; }
        public TimeSpan To { get; set; }
    }
}