using System;

namespace Sheaft.Mailing
{
    public class ExpectedOrderDeliveryMailerModel
    {
        public DateTimeOffset ExpectedDeliveryDate { get; set; }
        public DayOfWeek Day { get; set; }
        public TimeSpan From { get; set; }
        public TimeSpan To { get; set; }
    }
}