using System;
using System.Collections.Generic;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Common.Models.Dto
{
    public class DeliveryDto
    {
        public Guid Id { get; set; }
        public DeliveryKind Kind { get; set; }
        public string Name { get; set; }
        public bool Available { get; set; }
        public AddressDto Address { get; set; }
        public IEnumerable<DeliveryHourDto> DeliveryHours { get; set; }
        public IEnumerable<ClosingDto> Closings { get; set; }
    }
}