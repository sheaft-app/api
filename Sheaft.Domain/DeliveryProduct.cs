using System;
using Sheaft.Domain.Enum;

namespace Sheaft.Domain
{
    public class DeliveryProduct : ProductRow
    {
        protected DeliveryProduct()
        {
        }

        public DeliveryProduct(ProductRow product)
            : this(product, product.Quantity, ModificationKind.ToDeliver)
        {
        }

        public DeliveryProduct(ProductRow product, int quantity, ModificationKind kind)
            : base(product, quantity)
        {
            RowKind = kind;
        }

        public ModificationKind RowKind { get; private set; }
        public Guid DeliveryId { get; private set; }
    }
}