using System;

namespace Sheaft.Domain.Models
{
    public class OrderProduct : ProductRow
    {
        protected OrderProduct()
        {
        }

        public OrderProduct(Product product, int quantity)
            : base(product, quantity)
        {
            Producer = product.Producer;
        }

        public virtual User Producer { get; private set; }
    }
}