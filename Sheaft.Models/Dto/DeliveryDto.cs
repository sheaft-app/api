using Sheaft.Interop.Enums;
using System;
using System.Collections.Generic;

namespace Sheaft.Models.Dto
{
    public class DeliveryDto
    {
        public Guid Id { get; set; }
        public DeliveryKind Kind { get; set; }
        public string Name { get; set; }
        public AddressDto Address { get; set; }
        public IEnumerable<DeliveryHourDto> DeliveryHours { get; set; }
    }
}