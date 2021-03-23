using System.Collections.Generic;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Models
{
    public class CreateOrderDto
    {
        public DonationKind Donation { get; set; }
        public IEnumerable<ProducerExpectedDeliveryDto> ProducersExpectedDeliveries { get; set; }
        public IEnumerable<ResourceIdQuantityDto> Products { get; set; }
    }
}