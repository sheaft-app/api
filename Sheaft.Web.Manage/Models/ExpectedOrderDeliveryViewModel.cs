using System;

namespace Sheaft.Web.Manage.Models
{
    public class ExpectedOrderDeliveryViewModel
    {
        public DateTimeOffset ExpectedDeliveryDate { get; set; }
        public DayOfWeek Day { get; set; }
        public TimeSpan From { get; set; }
        public TimeSpan To { get; set; }
    }
}
