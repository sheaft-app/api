using Sheaft.Interop.Enums;
using System;
using System.Collections.Generic;

namespace Sheaft.Models.Dto
{
    public class AgreementDto
    {
        public Guid Id { get; set; }
        public AgreementStatusKind Status { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public DateTimeOffset? RemovedOn { get; set; }
        public string Reason { get; set; }
        public AgreementDeliveryModeDto Delivery { get; set; }
        public UserDto Store { get; set; }
        public IEnumerable<TimeSlotDto> SelectedHours { get; set; }
    }
}