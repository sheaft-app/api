using System;
using System.Collections.Generic;

namespace Sheaft.Application.Models
{
    public class ProducerDeliveriesDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<DeliveryDto> Deliveries { get; set; }
    }
}