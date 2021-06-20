using System;
using Sheaft.Domain.Enum;

namespace Sheaft.Domain
{
    public class OrderProduct : ProductRow
    {
        protected OrderProduct()
        {
        }

        public OrderProduct(Product product, Guid catalogId, int quantity)
            : base(product, catalogId, quantity, ModificationKind.Order)
        {
            Producer = product.Producer;
            ProducerId = product.Producer.Id;
        }

        public Guid OrderId { get; private set; }
        public Guid ProducerId { get; private set; }
        public virtual User Producer { get; private set; }
    }
}