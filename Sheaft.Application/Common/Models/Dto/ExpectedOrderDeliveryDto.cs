using Sheaft.Domain.Enums;
using System;

namespace Sheaft.Application.Models
{
    public class ExpectedOrderDeliveryDto
    {
        public DateTimeOffset ExpectedDeliveryDate { get; set; }
        public DayOfWeek Day { get; set; }
        public TimeSpan From { get; set; }
        public TimeSpan To { get; set; }
    }
}