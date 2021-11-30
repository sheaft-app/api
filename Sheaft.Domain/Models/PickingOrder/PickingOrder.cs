using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Domain.Common;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events;
using Sheaft.Domain.Events.Picking;
using Sheaft.Domain.Exceptions;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class PickingOrder : IEntity, IHasDomainEvent
    {
        protected PickingOrder()
        {
        }

        public PickingOrder(string name, Company supplier, IEnumerable<PurchaseOrder> purchaseOrders)
        {
            OrderStatus = PickingOrderStatus.Pending;
            SupplierId = supplier.Id;
            Supplier = supplier;

            SetName(name);
            AddPurchaseOrders(purchaseOrders);
        }

        public Guid Id { get; } = Guid.NewGuid();
        public string Name { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset UpdatedOn { get; private set; }
        public bool Removed { get; private set; }
        public PickingOrderStatus OrderStatus { get; private set; }
        public DateTimeOffset? CompletedOn { get; private set; }
        public string PickingFormUrl { get; private set; }
        public Guid SupplierId { get; private set; }
        public Guid? DeliveryId { get; private set; }
        public Company Supplier { get; private set; }
        public ICollection<PickingProduct> Products { get; private set; }
        public List<DomainEvent> DomainEvents { get; } = new List<DomainEvent>();
        
        public void Restore()
        {
            Removed = false;
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(Name))
                throw new ValidationException("Le nom de la préparation est requis.");

            if (string.IsNullOrWhiteSpace(name))
                name = $"Préparation du {DateTimeOffset.UtcNow:dd/MM/yyyy}";

            Name = name;
        }

        public void AddPurchaseOrders(IEnumerable<PurchaseOrder> purchaseOrders)
        {
            if (purchaseOrders == null || !purchaseOrders.Any())
                throw new ValidationException("La préparation requiert une commande à minima.");

            if (purchaseOrders.Any(po =>
                po.Status != PurchaseOrderStatus.Pending && po.Status != PurchaseOrderStatus.Accepted &&
                po.Status != PurchaseOrderStatus.Processing))
                throw new ValidationException(
                    "Seule des commandes en attente ou acceptées peuvent être ajoutées à une préparation.");

            if (OrderStatus == PickingOrderStatus.Completed)
                throw new ValidationException(
                    "Impossible de modifier les commandes d'une préparation qui est terminée.");

            foreach (var purchaseOrder in purchaseOrders)
            {
                if (purchaseOrder.Status == PurchaseOrderStatus.Pending)
                    purchaseOrder.Accept(true);
            }

            Products ??= new List<PickingProduct>();
            foreach (var purchaseOrder in purchaseOrders)
            {
                foreach (var purchaseOrderProduct in purchaseOrder.Products)
                    Products.Add(new PickingProduct(purchaseOrderProduct.ProductId, purchaseOrder.Id, purchaseOrderProduct.Quantity));
            }

            if (OrderStatus == PickingOrderStatus.InProgress)
                foreach (var purchaseOrder in purchaseOrders)
                    purchaseOrder.SetStatus(PurchaseOrderStatus.Processing, true);
        }

        public void RemovePurchaseOrders(IEnumerable<PurchaseOrder> purchaseOrders)
        {
            if (OrderStatus == PickingOrderStatus.Completed)
                throw new ValidationException(
                    "Impossible de modifier les commandes d'une préparation qui est terminée.");

            if (purchaseOrders == null || !purchaseOrders.Any())
                return;

            foreach (var purchaseOrder in purchaseOrders)
            {
                foreach (var purchaseOrderProduct in purchaseOrder.Products)
                {
                    var product = Products.FirstOrDefault(p =>
                        p.ProductId == purchaseOrderProduct.ProductId && p.PurchaseOrderId == purchaseOrder.Id);

                    if (product == null)
                        continue;

                    Products.Remove(product);

                    var preparedProduct = Products.FirstOrDefault(p =>
                        p.ProductId == purchaseOrderProduct.ProductId && p.PurchaseOrderId == purchaseOrder.Id);

                    if (preparedProduct == null)
                        continue;

                    Products.Remove(preparedProduct);
                }
                
                if (purchaseOrder.Status != PurchaseOrderStatus.Accepted)
                    purchaseOrder.SetStatus(PurchaseOrderStatus.Accepted, true);
            }
        }

        public void Start()
        {
            if (OrderStatus == PickingOrderStatus.Completed)
                throw new ValidationException("La préparation est déjà terminée.");

            OrderStatus = PickingOrderStatus.InProgress;
        }

        public void Complete()
        {
            if (OrderStatus == PickingOrderStatus.Completed)
                throw new ValidationException("La préparation est déjà terminée.");

            CompletedOn = DateTimeOffset.UtcNow;
            OrderStatus = PickingOrderStatus.Completed;
        }

        public void SetProductPreparedQuantity(Guid productId, Guid purchaseOrderId, int quantity, IEnumerable<BatchNumber> batches)
        {
            var productToPrepare = Products.SingleOrDefault(p =>
                p.ProductId == productId && p.PurchaseOrderId == purchaseOrderId);
            
            if (productToPrepare == null)
                throw new NotFoundException("Le produit est introuvable dans la préparation.");
            
            Products.Remove(productToPrepare);
            Products.Add(new PickingProduct(productToPrepare.ProductId, productToPrepare.PurchaseOrderId, productToPrepare.QuantityToPrepare, quantity, batches.Select(b => b.Id)));

        }

        public void SetPreparationUrl(string url)
        {
            PickingFormUrl = url;
            DomainEvents.Add(new PickingFormGeneratedEvent(Id));
        }

        public void ClearForm()
        {
            PickingFormUrl = null;
        }
    }
}