using System;

namespace Sheaft.Domain
{
    public class PreparedProduct : ProductRow
    {
        protected PreparedProduct()
        {
        }

        public PreparedProduct(ProductRow product, Guid purchaseOrderId)
            : this(product, purchaseOrderId, product.Quantity)
        {
        }

        public PreparedProduct(ProductRow product, Guid purchaseOrderId, int quantity)
            : base(product, quantity)
        {
            PurchaseOrderId = purchaseOrderId;
        }

        public Guid PickingId { get; private set; }
        public Guid PurchaseOrderId { get; private set; }
        public string PreparedBy { get; private set; }
        public DateTimeOffset? PreparedOn { get; private set; }

        public void CompleteProduct(string preparedBy)
        {
            PreparedBy = preparedBy;
            PreparedOn = DateTimeOffset.Now;
            
            RefreshLine();
        }
    }
}