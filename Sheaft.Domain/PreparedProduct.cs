using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Core.Exceptions;

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
        public virtual ICollection<PreparedProductBatch> Batches { get; private set; }

        public void CompleteProduct(string preparedBy)
        {
            PreparedBy = preparedBy;
            PreparedOn = DateTimeOffset.Now;

            RefreshLine();
        }

        public void SetBatches(IEnumerable<Batch> batches)
        {
            if(batches == null || !batches.Any())
                return;
            
            var existingBatchIds = Batches?.Select(b => b.BatchId).ToList() ?? new List<Guid>();
            var newBatchIds = batches.Select(b => b.Id);
            var batchIdsToRemove = existingBatchIds.Except(newBatchIds);
            
            if(batchIdsToRemove.Any())
                RemoveBatches(Batches?.Where(b => batchIdsToRemove.Contains(b.BatchId)).Select(b => b.Batch).ToList());
            
            existingBatchIds = Batches?.Select(b => b.BatchId).ToList() ?? new List<Guid>();
            var batchIdsToAdd = newBatchIds.Except(existingBatchIds);
            
            if(batchIdsToAdd.Any())
                AddBatches(batches.Where(b => batchIdsToAdd.Contains(b.Id)).ToList());
        }

        private void AddBatches(IEnumerable<Batch> batches)
        {
            if (Batches == null)
                Batches = new List<PreparedProductBatch>();

            foreach (var batch in batches)
                Batches.Add(new PreparedProductBatch(batch));
        }

        private void RemoveBatches(IEnumerable<Batch> batches)
        {
            if (Batches == null)
                throw SheaftException.NotFound("Ce produit ne contient pas de lots.");

            foreach (var batch in batches)
            {
                var productBatch = Batches.FirstOrDefault(b => b.BatchId == batch.Id);
                if (productBatch == null)
                    throw SheaftException.NotFound("Ce produit ne contient pas ce lot.");

                Batches.Remove(productBatch);
            }
        }
    }
}