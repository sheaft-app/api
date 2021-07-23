using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Core.Exceptions;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events.Observation;

namespace Sheaft.Domain
{
    public class Observation : Surveillance
    {
        protected Observation()
        {
        }

        public Observation(Guid id, string comment, User user, Producer producer)
            : base(id, comment, producer)
        {
            UserId = user.Id;
            User = user;
        }

        public bool VisibleToAll { get; private set; }
        public Guid? ReplyToId { get; private set; }
        public Guid UserId { get; private set; }
        public int RepliesCount { get; private set; }
        public int BatchesCount { get; private set; }
        public int ProductsCount { get; private set; }
        public virtual User User { get; private set; }
        public virtual ICollection<Observation> Replies { get; private set; }
        public virtual ICollection<ObservationBatch> Batches { get; private set; }
        public virtual ICollection<ObservationProduct> Products { get; private set; }

        private void Refresh()
        {
            RepliesCount = Replies?.Count ?? 0;
            BatchesCount = Batches?.Count ?? 0;
            ProductsCount = Products?.Count ?? 0;
        }
        
        public void AddReply(string comment, User user)
        {
            if (ReplyToId.HasValue)
                throw SheaftException.Validation("Une réponse ne peut être ajoutée qu'à une observation.");

            if (Replies == null)
                Replies = new List<Observation>();

            var reply = new Observation(Guid.NewGuid(), comment, user, Producer);
            Replies.Add(reply);

            DomainEvents.Add(new ObservationRepliedEvent(Id, reply.Id));
            Refresh();
        }

        public void SetVisibility(bool visibleToAll)
        {
            if (ReplyToId.HasValue && visibleToAll)
                throw SheaftException.Validation("Une réponse à une observation ne peut pas être publique.");

            if (User.Kind != ProfileKind.Producer && visibleToAll)
                throw SheaftException.Validation("Seule les observations d'un producteur peuvent être publique.");

            VisibleToAll = visibleToAll;
        }

        public void SetBatches(IEnumerable<Batch> batches)
        {
            if (batches == null || !batches.Any())
                return;
            
            if (batches.Any(p => p.ProducerId != ProducerId))
                throw SheaftException.BadRequest("Une observation est liée au producteur, les lots doivent donc lui être liés.");

            var existingBatchIds = Batches?.Select(b => b.BatchId).ToList() ?? new List<Guid>();
            var newBatchIds = batches.Select(b => b.Id);
            var batchIdsToRemove = existingBatchIds.Except(newBatchIds);

            if (batchIdsToRemove.Any())
                RemoveBatches(Batches?.Where(b => batchIdsToRemove.Contains(b.BatchId)).Select(b => b.Batch).ToList());

            existingBatchIds = Batches?.Select(b => b.BatchId).ToList() ?? new List<Guid>();
            var batchIdsToAdd = newBatchIds.Except(existingBatchIds);

            if (batchIdsToAdd.Any())
                AddBatches(batches.Where(b => batchIdsToAdd.Contains(b.Id)).ToList());
            
            Refresh();
        }

        public void SetProducts(IEnumerable<Product> products)
        {
            if (products == null || !products.Any())
                return;

            if (products.Any(p => p.ProducerId != ProducerId))
                throw SheaftException.BadRequest("Une observation est liée au producteur, les produits doivent donc lui être liés.");

            var existingProductIds = Products?.Select(b => b.ProductId).ToList() ?? new List<Guid>();
            var newProductIds = products.Select(b => b.Id);
            var productIdsToRemove = existingProductIds.Except(newProductIds);

            if (productIdsToRemove.Any())
                RemoveProducts(Products?.Where(b => productIdsToRemove.Contains(b.ProductId)).Select(b => b.Product)
                    .ToList());

            existingProductIds = Products?.Select(b => b.ProductId).ToList() ?? new List<Guid>();
            var productIdsToAdd = newProductIds.Except(existingProductIds);

            if (productIdsToAdd.Any())
                AddProducts(products.Where(b => productIdsToAdd.Contains(b.Id)).ToList());
            
            Refresh();
        }

        private void AddBatches(IEnumerable<Batch> batches)
        {
            if (Batches == null)
                Batches = new List<ObservationBatch>();

            foreach (var batch in batches)
                Batches.Add(new ObservationBatch(batch));
        }

        private void RemoveBatches(IEnumerable<Batch> batches)
        {
            if (Batches == null)
                throw SheaftException.NotFound("Cette observation ne contient pas de lots.");

            foreach (var batch in batches)
            {
                var observationBatch = Batches.FirstOrDefault(b => b.BatchId == batch.Id);
                if (observationBatch == null)
                    throw SheaftException.NotFound("Cette observation ne contient pas ce lot.");

                Batches.Remove(observationBatch);
            }
        }

        private void AddProducts(IEnumerable<Product> products)
        {
            if (Products == null)
                Products = new List<ObservationProduct>();

            foreach (var product in products)
                Products.Add(new ObservationProduct(product));
        }

        private void RemoveProducts(IEnumerable<Product> products)
        {
            if (Products == null)
                throw SheaftException.NotFound("Cette observation ne contient pas de produits.");

            foreach (var product in products)
            {
                var observationProduct = Products.FirstOrDefault(b => b.ProductId == product.Id);
                if (observationProduct == null)
                    throw SheaftException.NotFound("Cette observation ne contient pas ce produit.");

                Products.Remove(observationProduct);
            }
        }
    }
}