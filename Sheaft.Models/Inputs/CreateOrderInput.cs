using System.Collections.Generic;

namespace Sheaft.Models.Inputs
{
    public class CreateOrderInput
    {
        public IEnumerable<ProductQuantityInput> Products { get; set; }
        public IEnumerable<ProducerExpectedDeliveryInput> ProducersExpectedDeliveries { get; set; }
    }
}