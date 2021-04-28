using System;

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

        public Guid PurchaseOrderId { get; private set; }
    }
}