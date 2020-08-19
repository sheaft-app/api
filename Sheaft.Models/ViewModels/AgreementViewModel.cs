using Sheaft.Interop.Enums;
using System;
using System.Collections.Generic;

namespace Sheaft.Models.ViewModels
{
    public class AgreementViewModel
    {
        public Guid Id { get; set; }
        public AgreementStatusKind Status { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public DateTimeOffset? RemovedOn { get; set; }
        public string Reason { get; set; }
        public DeliveryModeViewModel Delivery { get; set; }
        public CompanyViewModel Store { get; set; }
        public IEnumerable<TimeSlotViewModel> SelectedHours { get; set; }
    }
}
