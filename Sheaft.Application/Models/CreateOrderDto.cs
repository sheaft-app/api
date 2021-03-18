using System.Collections.Generic;

namespace Sheaft.Application.Models
{
    public class CreateOrderDto
    {
        public decimal? Donation { get; set; }
        public IEnumerable<ProducerExpectedDeliveryDto> ProducersExpectedDeliveries { get; set; }
        public IEnumerable<ResourceIdQuantityDto> Products { get; set; }
    }
}