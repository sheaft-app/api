using System;
using System.Collections.Generic;

namespace Sheaft.Domain
{
    public class PickingProduct
    {
        protected PickingProduct()
        {
        }

        public PickingProduct(Guid productId, Guid purchaseOrderId, int quantityToPrepare, int? preparedQuantity = null, IEnumerable<Guid> batchNumbersIds = null)
        {
            ProductId = productId;
            PurchaseOrderId = purchaseOrderId;
            QuantityToPrepare = quantityToPrepare;
            PreparedQuantity = preparedQuantity;

            AddBatches(batchNumbersIds);
        }

        public int QuantityToPrepare { get; }
        public int? PreparedQuantity { get; private set; }
        public Guid ProductId { get; }
        public Guid PurchaseOrderId { get; }
        public ICollection<PickingBatchNumber> BatchNumbers { get; private set; }

        private void AddBatches(IEnumerable<Guid> batchNumbersIds)
        {
            if (BatchNumbers == null)
                BatchNumbers = new List<PickingBatchNumber>();

            foreach (var batchNumberId in batchNumbersIds)
                BatchNumbers.Add(new PickingBatchNumber(batchNumberId));
        }
    }
}