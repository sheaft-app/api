using System;
using Sheaft.Domain.Enum;

namespace Sheaft.Domain
{
    public class DeliveryProduct : ProductRow
    {
        protected DeliveryProduct()
        {
        }

        public DeliveryProduct(ProductRow product, Guid purchaseOrderId)
            : this(product, product.Quantity, ModificationKind.Deliver, purchaseOrderId)
        {
        }

        public DeliveryProduct(ProductRow product, int quantity, ModificationKind kind, Guid purchaseOrderId)
            : base(product, quantity, kind)
        {
            PurchaseOrderId = purchaseOrderId;
        }

        public DeliveryProduct(DeliveryProduct product, int quantity, ModificationKind kind)
            : base(product, quantity, kind)
        {
            PurchaseOrderId = product.PurchaseOrderId;
        }

        public Guid DeliveryId { get; private set; }
        public Guid PurchaseOrderId { get; private set; }
    }
}