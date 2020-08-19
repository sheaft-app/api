using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Models.ViewModels
{
    public class ExpectedDeliveryViewModel
    {
        public DateTimeOffset ExpectedDeliveryDate { get; set; }
        public DateTimeOffset? DeliveryStartedOn { get; set; }
        public DateTimeOffset? DeliveredOn { get; set; }
        public DayOfWeek Day { get; set; }
        public TimeSpan From { get; set; }
        public TimeSpan To { get; set; }
        public AddressViewModel Address { get; set; }
        public string Name { get; set; }
        public DeliveryKind Kind { get; set; }
    }
}
