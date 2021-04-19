using System;

namespace Sheaft.Application.Models
{
    public class ProducerExpectedDeliveryInputDto
    {
        public Guid ProducerId { get; set; }
        public Guid DeliveryModeId { get; set; }
        public DateTimeOffset ExpectedDeliveryDate { get; set; }
        public string Comment { get; set; }
    }
}