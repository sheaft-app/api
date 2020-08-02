using System;

namespace Sheaft.Models.Inputs
{
    public class ProducerExpectedDeliveryInput
    {
        public Guid ProducerId { get; set; }
        public Guid DeliveryModeId { get; set; }
        public DateTimeOffset ExpectedDeliveryDate { get; set; }
        public string Comment { get; set; }
    }
}
