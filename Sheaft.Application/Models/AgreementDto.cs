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
        public DeliveryModeDto Delivery { get; set; }
        public UserDto Store { get; set; }
        public UserDto Producer { get; set; }
        public ProfileKind CreatedByKind { get; set; }
        public CatalogDto Catalog { get; set; }
    }
}