using System;

namespace Sheaft.Models.Dto
{
    public class DeliveryHourDto
    {
        public DateTimeOffset ExpectedDeliveryDate { get; set; }

        public DayOfWeek Day { get; set; }
        public TimeSpan From { get; set; }
        public TimeSpan To { get; set; }
    }
}