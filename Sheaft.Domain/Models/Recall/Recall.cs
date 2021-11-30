using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Domain.BaseClass;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Exceptions;

namespace Sheaft.Domain
{
    public class Recall : Surveillance
    {
        protected Recall()
        {
        }

        public Recall(string name, DateTimeOffset saleStartedOn, DateTimeOffset saleEndedOn, string comment,
            Company supplier, IEnumerable<Product> products)
            : base(comment, supplier)
        {
            if (saleStartedOn > saleEndedOn)
                throw new ValidationException(
                    "La date de fin de commercialisation ne peut pas être antérieure à la date de début de commercialisation.");

            if (products == null || !products.Any())
                throw new ValidationException(
                    "Vous devez spécifier au moins un produit pour une campagne de rappel.");
            
            SetName(name);
            SetSaleStartedOn(saleStartedOn);
            SetSaleEndedOn(saleEndedOn);
            SetProducts(products);
            Status = RecallStatus.Pending;
        }

        public string Name { get;private set; }
        public RecallStatus Status { get;private set; }
        public DateTimeOffset SaleStartedOn { get;private set; }
        public DateTimeOffset SaleEndedOn { get;private set; }
        public DateTimeOffset? SendingStartedOn { get;private set; }
        public DateTimeOffset? SendCompletedOn { get;private set; }
        public ICollection<RecallBatchNumber> Batches { get; private set; }
        public ICollection<RecallProduct> Products { get; private set; }
        public ICollection<RecallClient> Clients { get; private set; }

        public void SetBatches(IEnumerable<BatchNumber> batches)
        {
            if (batches == null || !batches.Any())
                return;

            if (batches.Any(p => p.SupplierId != SupplierId))
                throw new BadRequestException("Une campagne de rappel est liée au producteur, les lots doivent donc lui être liés.");

            var existingBatchIds = Batches?.Select(b => b.BatchNumberId).ToList() ?? new List<Guid>();
            var newBatchIds = batches.Select(b => b.Id);
            var batchIdsToRemove = existingBatchIds.Except(newBatchIds);

            if (batchIdsToRemove.Any())
                RemoveBatches(Batches?.Where(b => batchIdsToRemove.Contains(b.BatchNumberId)).Select(b => b.BatchNumberId).ToList());

            existingBatchIds = Batches?.Select(b => b.BatchNumberId).ToList() ?? new List<Guid>();
            var batchIdsToAdd = newBatchIds.Except(existingBatchIds);

            if (batchIdsToAdd.Any())
                AddBatches(batches.Where(b => batchIdsToAdd.Contains(b.Id)).ToList());
        }

        public void SetProducts(IEnumerable<Product> products)
        {
            if (products == null || !products.Any())
                return;

            if (products.Any(p => p.SupplierId != SupplierId))
                throw new BadRequestException("Une campagne de rappel est liée au producteur, les produits doivent donc lui être liés.");

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

        public void SetClients(IEnumerable<User> clients)
        {
            if (clients == null || !clients.Any())
                return;

            var existingClientIds = Clients?.Select(b => b.ClientId).ToList() ?? new List<Guid>();
            var newClientIds = clients.Select(b => b.Id);
            var clientIdsToRemove = existingClientIds.Except(newClientIds);

            if (clientIdsToRemove.Any())
                RemoveClients(Clients?.Where(b => clientIdsToRemove.Contains(b.ClientId)).Select(b => b.ClientId)
                    .ToList());

            existingClientIds = Clients?.Select(b => b.ClientId).ToList() ?? new List<Guid>();
            var clientIdsToAdd = newClientIds.Except(existingClientIds);

            if (clientIdsToAdd.Any())
                AddClients(clients.Where(b => clientIdsToAdd.Contains(b.Id)).ToList());
        }

        private void AddBatches(IEnumerable<BatchNumber> batches)
        {
            if (Batches == null)
                Batches = new List<RecallBatchNumber>();

            foreach (var batch in batches)
                Batches.Add(new RecallBatchNumber(batch));
        }

        private void RemoveBatches(IEnumerable<Guid> batches)
        {
            if (Batches == null)
                throw new NotFoundException("Ce rappel ne contient pas de lots.");

            foreach (var batch in batches)
            {
                var observationBatch = Batches.FirstOrDefault(b => b.BatchNumberId == batch);
                if (observationBatch == null)
                    throw new NotFoundException("Ce rappel ne contient pas ce lot.");

                Batches.Remove(observationBatch);
            }
        }

        private void AddProducts(IEnumerable<Product> products)
        {
            if (Products == null)
                Products = new List<RecallProduct>();

            foreach (var product in products)
                Products.Add(new RecallProduct(product));
        }

        private void RemoveProducts(IEnumerable<Guid> productsIds)
        {
            if (Products == null)
                throw new NotFoundException("Ce rappel ne contient pas de produits.");

            foreach (var productId in productsIds)
            {
                var observationProduct = Products.FirstOrDefault(b => b.ProductId == productId);
                if (observationProduct == null)
                    throw new NotFoundException("Ce rappel ne contient pas ce produit.");

                Products.Remove(observationProduct);
            }
        }

        private void AddClients(IEnumerable<User> clients)
        {
            if (Clients == null)
                Clients = new List<RecallClient>();

            foreach (var client in clients)
                Clients.Add(new RecallClient(client));
        }

        private void RemoveClients(IEnumerable<Guid> clients)
        {
            if (Clients == null)
                throw new NotFoundException("Ce rappel ne contient pas de clients.");

            foreach (var client in clients)
            {
                var observationClient = Clients.FirstOrDefault(b => b.ClientId == client);
                if (observationClient == null)
                    throw new NotFoundException("Ce rappel ne contient pas ce client.");

                Clients.Remove(observationClient);
            }
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ValidationException("Le nom est requis.");

            Name = name;
        }

        public void SetSaleStartedOn(DateTimeOffset saleStartedOn)
        {
            SaleStartedOn = saleStartedOn;
        }

        public void SetSaleEndedOn(DateTimeOffset saleEndedOn)
        {
            SaleEndedOn = saleEndedOn;
        }

        public void StartSending()
        {
            if (Status != RecallStatus.Ready)
                throw new ValidationException("La campagne n'est pas prête pour l'envoi.");
            
            Status = RecallStatus.Sending;
            SendingStartedOn ??= DateTimeOffset.UtcNow;
        }

        public void SetAsReady()
        {
            if (Status != RecallStatus.Pending)
                throw new ValidationException("La campagne n'est pas en attente.");
            
            Status = RecallStatus.Ready;
        }

        public void CompleteSending()
        {
            Status = RecallStatus.Sent;
            SendCompletedOn = DateTimeOffset.UtcNow;
        }

        public void FailSending()
        {
            Status = RecallStatus.Failed;
        }
    }
}