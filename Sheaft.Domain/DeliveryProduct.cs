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
            SetQuantity(quantity);
        }

        public ModificationKind RowKind { get; private set; }
        public Guid DeliveryId { get; private set; }
        
        protected sealed override void SetQuantity(int quantity)
        {
            if(RowKind is ModificationKind.ToDeliver or ModificationKind.Excess)
                Quantity = quantity;
            else
                Quantity = -quantity;
            
            RefreshLine();
        }
    }
}