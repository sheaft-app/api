using System;
using System.Collections.Generic;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Models
{
    public class AgreementDto
    {
        public Guid Id { get; set; }
        public AgreementStatus Status { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public DateTimeOffset? RemovedOn { get; set; }
        public string Reason { get; set; }
        public AgreementDeliveryModeDto Delivery { get; set; }
        public UserDto Store { get; set; }
        public AgreementCatalogDto Catalog { get; set; }
        public IEnumerable<TimeSlotDto> SelectedHours { get; set; }
    }
}