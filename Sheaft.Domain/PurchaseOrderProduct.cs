using System;
using Sheaft.Domain.Enum;

namespace Sheaft.Domain
{
    public class PurchaseOrderProduct : ProductRow
    {
        protected PurchaseOrderProduct()
        {
        }

        public PurchaseOrderProduct(ProductRow product)
            : base(product)
        {
        }

        public PurchaseOrderProduct(ProductRow product, int quantity, ModificationKind kind)
            : base(product, quantity, kind)
        {
        }

        public Guid PurchaseOrderId { get; private set; }
    }
}