using System;

namespace Sheaft.Domain
{
    public class PickingProduct : ProductRow
    {
        protected PickingProduct()
        {
        }

        public PickingProduct(ProductRow product, Guid purchaseOrderId)
            : this(product, purchaseOrderId, product.Quantity)
        {
        }

        public PickingProduct(ProductRow product, Guid purchaseOrderId, int quantity)
            : base(product, quantity)
        {
            PurchaseOrderId = purchaseOrderId;
        }

        public Guid PickingId { get; private set; }
        public Guid PurchaseOrderId { get; private set; }
    }
}