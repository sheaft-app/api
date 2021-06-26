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

        public PurchaseOrderProduct(ProductRow product, int quantity)
            : base(product, quantity)
        {
        }

        public Guid PurchaseOrderId { get; private set; }
    }
}