using System;

namespace Sheaft.Application.Models
{
    public class ExpectedDeliveryDto
    { 
        public DateTimeOffset ExpectedDeliveryDate { get; set; }
        public DayOfWeek Day { get; set; }
        public TimeSpan From { get; set; }
        public TimeSpan To { get; set; }
    }
}