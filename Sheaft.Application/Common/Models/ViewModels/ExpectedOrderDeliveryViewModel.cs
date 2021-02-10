using System;

namespace Sheaft.Application.Common.Models.ViewModels
{
    public class ExpectedOrderDeliveryViewModel
    {
        public DateTimeOffset ExpectedDeliveryDate { get; set; }
        public DayOfWeek Day { get; set; }
        public TimeSpan From { get; set; }
        public TimeSpan To { get; set; }
    }
}
