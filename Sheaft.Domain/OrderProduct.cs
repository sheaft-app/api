using System;

namespace Sheaft.Domain
{
    public class OrderProduct : ProductRow
    {
        protected OrderProduct()
        {
        }

        public OrderProduct(Product product, Guid catalogId, int quantity)
            : base(product, catalogId, quantity)
        {
            Producer = product.Producer;
        }

        public virtual User Producer { get; private set; }
    }
}