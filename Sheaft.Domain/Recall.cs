using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Core.Exceptions;
using Sheaft.Domain.Enum;

namespace Sheaft.Domain
{
    public class Recall : Surveillance
    {
        protected Recall()
        {
        }

        public Recall(Guid id, string name, DateTimeOffset saleStartedOn, DateTimeOffset saleEndedOn, string comment,
            Producer producer, IEnumerable<Product> products)
            : base(id, comment, producer)
        {
            if (saleStartedOn > saleEndedOn)
                throw SheaftException.Validation(
                    "La date de fin de commercialisation ne peut pas être antérieure à la date de début de commercialisation.");

            if (products == null || !products.Any())
                throw SheaftException.Validation(
                    "Vous devez spécifier au moins un produit pour une campagne de rappel.");
            
            SetName(name);
            SetSaleStartedOn(saleStartedOn);
            SetSaleEndedOn(saleEndedOn);
            SetProducts(products);
            Status = RecallStatus.Waiting;
        }

        public string Name { get;private set; }
        public RecallStatus Status { get;private set; }
        public DateTimeOffset SaleStartedOn { get;private set; }
        public DateTimeOffset SaleEndedOn { get;private set; }
        public DateTimeOffset? SendingStartedOn { get;private set; }
        public DateTimeOffset? SendCompletedOn { get;private set; }
        public int BatchesCount { get;private set; }
        public int ProductsCount { get;private set; }
        public int ClientsCount { get;private set; }
        public virtual ICollection<RecallBatch> Batches { get; private set; }
        public virtual ICollection<RecallProduct> Products { get; private set; }
        public virtual ICollection<RecallClient> Clients { get; private set; }

        private void Refresh()
        {
            ClientsCount = Clients?.Count ?? 0;
            BatchesCount = Batches?.Count ?? 0;
            ProductsCount = Products?.Count ?? 0;
        }
        
        public void SetBatches(IEnumerable<Batch> batches)
        {
            if (batches == null || !batches.Any())
                return;

            if (batches.Any(p => p.ProducerId != ProducerId))
                throw SheaftException.BadRequest("Une campagne de rappel est liée au producteur, les lots doivent donc lui être liés.");

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
                throw SheaftException.BadRequest("Une campagne de rappel est liée au producteur, les produits doivent donc lui être liés.");

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

        public void SetClients(IEnumerable<User> clients)
        {
            if (clients == null || !clients.Any())
                return;

            var existingClientIds = Clients?.Select(b => b.ClientId).ToList() ?? new List<Guid>();
            var newClientIds = clients.Select(b => b.Id);
            var clientIdsToRemove = existingClientIds.Except(newClientIds);

            if (clientIdsToRemove.Any())
                RemoveClients(Clients?.Where(b => clientIdsToRemove.Contains(b.ClientId)).Select(b => b.Client)
                    .ToList());

            existingClientIds = Clients?.Select(b => b.ClientId).ToList() ?? new List<Guid>();
            var clientIdsToAdd = newClientIds.Except(existingClientIds);

            if (clientIdsToAdd.Any())
                AddClients(clients.Where(b => clientIdsToAdd.Contains(b.Id)).ToList());
            
            Refresh();
        }

        private void AddBatches(IEnumerable<Batch> batches)
        {
            if (Batches == null)
                Batches = new List<RecallBatch>();

            foreach (var batch in batches)
                Batches.Add(new RecallBatch(batch));
        }

        private void RemoveBatches(IEnumerable<Batch> batches)
        {
            if (Batches == null)
                throw SheaftException.NotFound("Ce rappel ne contient pas de lots.");

            foreach (var batch in batches)
            {
                var observationBatch = Batches.FirstOrDefault(b => b.BatchId == batch.Id);
                if (observationBatch == null)
                    throw SheaftException.NotFound("Ce rappel ne contient pas ce lot.");

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

        private void RemoveProducts(IEnumerable<Product> products)
        {
            if (Products == null)
                throw SheaftException.NotFound("Ce rappel ne contient pas de produits.");

            foreach (var product in products)
            {
                var observationProduct = Products.FirstOrDefault(b => b.ProductId == product.Id);
                if (observationProduct == null)
                    throw SheaftException.NotFound("Ce rappel ne contient pas ce produit.");

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

        private void RemoveClients(IEnumerable<User> clients)
        {
            if (Clients == null)
                throw SheaftException.NotFound("Ce rappel ne contient pas de clients.");

            foreach (var client in clients)
            {
                var observationClient = Clients.FirstOrDefault(b => b.ClientId == client.Id);
                if (observationClient == null)
                    throw SheaftException.NotFound("Ce rappel ne contient pas ce client.");

                Clients.Remove(observationClient);
            }
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw SheaftException.Validation("Le nom est requis.");

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
                throw SheaftException.Validation("La campagne n'est pas prête pour l'envoi.");
            
            Status = RecallStatus.Sending;
            SendingStartedOn ??= DateTimeOffset.UtcNow;
        }

        public void SetAsReady()
        {
            if (Status != RecallStatus.Waiting)
                throw SheaftException.Validation("La campagne n'est pas en attente.");
            
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