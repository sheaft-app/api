using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Domain.BaseClass;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events.Observation;
using Sheaft.Domain.Exceptions;

namespace Sheaft.Domain
{
    public class Observation : Surveillance
    {
        protected Observation()
        {
        }

        public Observation(string comment, User user, Company supplier)
            : base(comment, supplier)
        {
            UserId = user.Id;
            User = user;   
        }

        public bool VisibleToAll { get; private set; }
        public Guid? ReplyToId { get; private set; }
        public Guid UserId { get; private set; }
        public User User { get; private set; }
        public ICollection<Observation> Replies { get; private set; }
        public ICollection<ObservationBatchNumber> BatchNumbers { get; private set; }
        public ICollection<ObservationProduct> Products { get; private set; }
                
        public void AddReply(string comment, User user)
        {
            if (ReplyToId.HasValue)
                throw new ValidationException("Une réponse ne peut être ajoutée qu'à une observation.");

            if (Replies == null)
                Replies = new List<Observation>();

            var reply = new Observation(comment, user, Supplier);
            Replies.Add(reply);

            DomainEvents.Add(new ObservationRepliedEvent(Id, reply.Id));
        }

        public void SetVisibility(bool visibleToAll)
        {
            if (ReplyToId.HasValue && visibleToAll)
                throw new ValidationException("Une réponse à une observation ne peut pas être publique.");

            VisibleToAll = visibleToAll;
        }

        public void SetBatches(IEnumerable<BatchNumber> batches)
        {
            if (batches == null || !batches.Any())
                return;
            
            if (batches.Any(p => p.SupplierId != SupplierId))
                throw new BadRequestException("Une observation est liée au producteur, les lots doivent donc lui être liés.");

            var existingBatchIds = BatchNumbers?.Select(b => b.BatchNumberId).ToList() ?? new List<Guid>();
            var newBatchIds = batches.Select(b => b.Id);
            var batchIdsToRemove = existingBatchIds.Except(newBatchIds);

            if (batchIdsToRemove.Any())
                RemoveBatches(BatchNumbers?.Where(b => batchIdsToRemove.Contains(b.BatchNumberId)).Select(b => b.BatchNumberId).ToList());

            existingBatchIds = BatchNumbers?.Select(b => b.BatchNumberId).ToList() ?? new List<Guid>();
            var batchIdsToAdd = newBatchIds.Except(existingBatchIds);

            if (batchIdsToAdd.Any())
                AddBatches(batches.Where(b => batchIdsToAdd.Contains(b.Id)).ToList());
        }

        public void SetProducts(IEnumerable<Product> products)
        {
            if (products == null || !products.Any())
                return;

            if (products.Any(p => p.SupplierId != SupplierId))
                throw new BadRequestException("Une observation est liée au producteur, les produits doivent donc lui être liés.");

            var existingProductIds = Products?.Select(b => b.ProductId).ToList() ?? new List<Guid>();
            var newProductIds = products.Select(b => b.Id);
            var productIdsToRemove = existingProductIds.Except(newProductIds);

            if (productIdsToRemove.Any())
                RemoveProducts(Products?.Where(b => productIdsToRemove.Contains(b.ProductId)).Select(b => b.ProductId)
                    .ToList());

            existingProductIds = Products?.Select(b => b.ProductId).ToList() ?? new List<Guid>();
            var productIdsToAdd = newProductIds.Except(existingProductIds);

            if (productIdsToAdd.Any())
                AddProducts(products.Where(b => productIdsToAdd.Contains(b.Id)).ToList());
        }

        private void AddBatches(IEnumerable<BatchNumber> batches)
        {
            if (BatchNumbers == null)
                BatchNumbers = new List<ObservationBatchNumber>();

            foreach (var batch in batches)
                BatchNumbers.Add(new ObservationBatchNumber(batch));
        }

        private void RemoveBatches(IEnumerable<Guid> batchNumberIds)
        {
            if (BatchNumbers == null)
                throw new NotFoundException("Cette observation ne contient pas de lots.");

            foreach (var batch in batchNumberIds)
            {
                var observationBatch = BatchNumbers.FirstOrDefault(b => b.BatchNumberId == batch);
                if (observationBatch == null)
                    throw new NotFoundException("Cette observation ne contient pas ce lot.");

                BatchNumbers.Remove(observationBatch);
            }
        }

        private void AddProducts(IEnumerable<Product> products)
        {
            if (Products == null)
                Products = new List<ObservationProduct>();

            foreach (var product in products)
                Products.Add(new ObservationProduct(product));
        }

        private void RemoveProducts(IEnumerable<Guid> productsIds)
        {
            if (Products == null)
                throw new NotFoundException("Cette observation ne contient pas de produits.");

            foreach (var productId in productsIds)
            {
                var observationProduct = Products.FirstOrDefault(b => b.ProductId == productId);
                if (observationProduct == null)
                    throw new NotFoundException("Cette observation ne contient pas ce produit.");

                Products.Remove(observationProduct);
            }
        }
    }
}