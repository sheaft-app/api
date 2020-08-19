using Sheaft.Interop.Enums;
using System;
using System.Collections.Generic;

namespace Sheaft.Models.ViewModels
{
    public class DeliveryModeViewModel
    {
        public Guid Id { get; set; }
        public DeliveryKind Kind { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public DateTimeOffset? RemovedOn { get; set; }
        public int LockOrderHoursBeforeDelivery { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public AddressViewModel Address { get; set; }
        public CompanyViewModel Producer { get; set; }
        public IEnumerable<TimeSlotViewModel> OpeningHours { get; set; }
    }
}
