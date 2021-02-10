using System;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Common.Models.Dto
{
    public class AgreementDeliveryModeDto
    {
        public Guid Id { get; set; }
        public DeliveryKind Kind { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public DateTimeOffset? RemovedOn { get; set; }
        public int LockOrderHoursBeforeDelivery { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public AddressDto Address { get; set; }
        public UserDto Producer { get; set; }
    }
}