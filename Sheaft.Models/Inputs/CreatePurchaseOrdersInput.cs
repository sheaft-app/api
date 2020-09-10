using System.Collections.Generic;

namespace Sheaft.Models.Inputs
{
    public class CreatePurchaseOrdersInput
    {
        public decimal? Donation { get; set; }
        public IEnumerable<ProductQuantityInput> Products { get; set; }
        public IEnumerable<ProducerExpectedDeliveryInput> ProducersExpectedDeliveries { get; set; }
    }
}