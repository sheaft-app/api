using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Core.Exceptions;
using Sheaft.Domain.Common;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events.Picking;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class Picking : IEntity, IHasDomainEvent
    {
        protected Picking()
        {
        }

        public Picking(Guid id, string name, Producer producer, IEnumerable<PurchaseOrder> purchaseOrders)
        {
            Id = id;
            Status = PickingStatus.Waiting;

            ProducerId = producer.Id;
            Producer = producer;

            SetName(name);
            AddPurchaseOrders(purchaseOrders);
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public DateTimeOffset? RemovedOn { get; private set; }
        public PickingStatus Status { get; private set; }
        public DateTimeOffset? StartedOn { get; private set; }
        public DateTimeOffset? CompletedOn { get; private set; }
        public int PurchaseOrdersCount { get; private set; }
        public int ProductsToPrepareCount { get; private set; }
        public int PreparedProductsCount { get; private set; }
        public string PickingFormUrl { get; private set; }
        public Guid ProducerId { get; private set; }
        public virtual Producer Producer { get; private set; }
        public virtual ICollection<PurchaseOrder> PurchaseOrders { get; private set; }
        public virtual ICollection<PickingProduct> ProductsToPrepare { get; private set; }
        public virtual ICollection<PreparedProduct> PreparedProducts { get; private set; }
        public byte[] RowVersion { get; set; }
        public List<DomainEvent> DomainEvents { get; } = new List<DomainEvent>();

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(Name))
                throw SheaftException.Validation("Le nom de la préparation est requis.");

            if (string.IsNullOrWhiteSpace(name))
                name = $"Préparation du {DateTimeOffset.UtcNow:dd/MM/yyyy}";

            Name = name;
        }

        public void AddPurchaseOrders(IEnumerable<PurchaseOrder> purchaseOrders)
        {
            if (purchaseOrders == null || !purchaseOrders.Any())
                throw SheaftException.Validation("La préparation requiert une commande à minima.");

            if (purchaseOrders.Any(po =>
                po.Status != PurchaseOrderStatus.Waiting && po.Status != PurchaseOrderStatus.Accepted && po.Status != PurchaseOrderStatus.Processing))
                throw SheaftException.Validation(
                    "Seule des commandes en attente ou acceptées peuvent être ajoutées à une préparation.");

            if (Status == PickingStatus.Completed)
                throw SheaftException.Validation(
                    "Impossible de modifier les commandes d'une préparation qui est terminée.");

            PurchaseOrders ??= new List<PurchaseOrder>();
            foreach (var purchaseOrder in purchaseOrders)
            {
                if (purchaseOrder.PickingId.HasValue)
                    purchaseOrder.Picking.RemovePurchaseOrders(new List<PurchaseOrder> {purchaseOrder});

                if(purchaseOrder.Status == PurchaseOrderStatus.Waiting)
                    purchaseOrder.Accept(true);
                
                PurchaseOrders.Add(purchaseOrder);
            }

            ProductsToPrepare ??= new List<PickingProduct>();
            foreach (var purchaseOrder in purchaseOrders)
            {
                foreach (var purchaseOrderProduct in purchaseOrder.Products)
                    ProductsToPrepare.Add(new PickingProduct(purchaseOrderProduct, purchaseOrder.Id,
                        purchaseOrderProduct.Quantity));
            }

            if (Status == PickingStatus.InProgress)
                foreach (var purchaseOrder in purchaseOrders)
                    purchaseOrder.SetStatus(PurchaseOrderStatus.Processing, true);

            Refresh();
        }

        public void RemovePurchaseOrders(IEnumerable<PurchaseOrder> purchaseOrders)
        {
            if (Status == PickingStatus.Completed)
                throw SheaftException.Validation(
                    "Impossible de modifier les commandes d'une préparation qui est terminée.");

            if (PurchaseOrders == null)
                throw SheaftException.NotFound("Cette préparation ne contient pas de commandes.");

            if (purchaseOrders == null || !purchaseOrders.Any())
                return;

            foreach (var purchaseOrder in purchaseOrders)
            {
                foreach (var purchaseOrderProduct in purchaseOrder.Products)
                {
                    var product = ProductsToPrepare.FirstOrDefault(p =>
                        p.ProductId == purchaseOrderProduct.ProductId && p.PurchaseOrderId == purchaseOrder.Id);

                    if (product == null)
                        continue;

                    ProductsToPrepare.Remove(product);

                    var preparedProduct = PreparedProducts.FirstOrDefault(p =>
                        p.ProductId == purchaseOrderProduct.ProductId && p.PurchaseOrderId == purchaseOrder.Id);

                    if (preparedProduct == null)
                        continue;

                    PreparedProducts.Remove(preparedProduct);
                }

                PurchaseOrders.Remove(purchaseOrder);
                purchaseOrder.SetStatus(PurchaseOrderStatus.Accepted, true);
            }

            Refresh();
        }

        public void Start()
        {
            if (Status == PickingStatus.Completed)
                throw SheaftException.Validation("La préparation est déjà terminée.");

            StartedOn ??= DateTimeOffset.UtcNow;
            Status = PickingStatus.InProgress;

            foreach (var purchaseOrder in PurchaseOrders)
                purchaseOrder.SetStatus(PurchaseOrderStatus.Processing, true);
        }

        public void Pause()
        {
            if (Status != PickingStatus.InProgress)
                throw SheaftException.Validation("La préparation n'est pas en cours de traitement.");

            Status = PickingStatus.Paused;
        }

        public void Complete()
        {
            if (Status == PickingStatus.Completed)
                throw SheaftException.Validation("La préparation est déjà terminée.");

            StartedOn ??= DateTimeOffset.UtcNow;
            CompletedOn = DateTimeOffset.UtcNow;
            Status = PickingStatus.Completed;
        }

        public void SetProductPreparedQuantity(Guid productId, Guid purchaseOrderId, int quantity, string preparedBy,
            bool completed, IEnumerable<Batch> batches)
        {
            var existingPreparedProduct =
                PreparedProducts.SingleOrDefault(p => p.ProductId == productId && p.PurchaseOrderId == purchaseOrderId);
            if (existingPreparedProduct != null)
            {
                existingPreparedProduct.SetBatches(batches);
                existingPreparedProduct.SetQuantity(quantity);
            }
            else
            {
                var productToPrepare = ProductsToPrepare.SingleOrDefault(p =>
                    p.ProductId == productId && p.PurchaseOrderId == purchaseOrderId);
                if (productToPrepare == null)
                    throw SheaftException.NotFound("Le produit est introuvable dans la préparation.");

                existingPreparedProduct = new PreparedProduct(productToPrepare, purchaseOrderId, quantity);
                existingPreparedProduct.SetBatches(batches);
                PreparedProducts.Add(existingPreparedProduct);
            }
            
            if (completed)
                existingPreparedProduct.CompleteProduct(preparedBy);

            Refresh();
            
            if (PreparedProducts.Count == ProductsToPrepare.Count && PreparedProducts.All(p => p.PreparedOn.HasValue))
                Complete();
        }

        public void SetPreparationUrl(string url)
        {
            PickingFormUrl = url;
            DomainEvents.Add(new PickingFormGeneratedEvent(Id));
        }

        private void Refresh()
        {
            ProductsToPrepareCount = ProductsToPrepare?.Sum(p => p.Quantity) ?? 0;
            PurchaseOrdersCount = PurchaseOrders.Count;
            PreparedProductsCount = PreparedProducts?.Sum(p => p.Quantity) ?? 0;
        }
    }
}