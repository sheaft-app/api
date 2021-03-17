using System;
using System.Collections.Generic;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Common.Models.ViewModels
{
    public class AgreementViewModel
    {
        public Guid Id { get; set; }
        public AgreementStatus Status { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public DateTimeOffset? RemovedOn { get; set; }
        public string Reason { get; set; }
        public DeliveryModeViewModel Delivery { get; set; }
        public StoreViewModel Store { get; set; }
        public IEnumerable<TimeSlotViewModel> SelectedHours { get; set; }
    }
}
