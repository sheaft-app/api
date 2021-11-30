using System;
using Sheaft.Domain.Enum;

namespace Sheaft.Domain
{
    public class DeliveryProduct
    {
        protected DeliveryProduct()
        {
        }

        public DeliveryProduct(Guid productId, int expectedQuantity, int? deliveredQuantity = 0, Guid? returnableId = null)
        {
            ProductId = productId;
            ReturnableId = returnableId;
            ExpectedQuantity = expectedQuantity;
            DeliveredQuantity = deliveredQuantity;
        }

        public int ExpectedQuantity { get; set; }
        public int? DeliveredQuantity { get; set; }
        public Guid ProductId { get; private set; }
        public Guid? ReturnableId { get; private set; }
    }
}