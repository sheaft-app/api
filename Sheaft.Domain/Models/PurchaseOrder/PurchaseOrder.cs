using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Domain.Common;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events;
using Sheaft.Domain.Events.PurchaseOrder;
using Sheaft.Domain.Exceptions;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class PurchaseOrder : IEntity, IHasDomainEvent
    {
        private const int DIGITS_COUNT = 2;
        private string _identifier;

        protected PurchaseOrder()
        {
        }

        public PurchaseOrder(Guid cartId, int reference, PurchaseOrderStatus status, Vendor vendor, Sender sender, PurchaseOrderDelivery expectedDelivery, IEnumerable<Tuple<Product, CatalogProductPrice, int>> products, string comment = null)
        {
            Reference = reference.ToString();
            Sender = sender;
            Vendor = vendor;
            CartId = cartId;
            ExpectedDelivery = expectedDelivery;
            Comment = comment;
            Lines = new List<PurchaseOrderLine>();
            DomainEvents = new List<DomainEvent> {new PurchaseOrderCreatedEvent(Id)};
            
            SetStatus(status, true);

            foreach (var productWithPriceAndQuantity in products)
                AddProduct(productWithPriceAndQuantity);
            
            // var shouldAddDeliveryFees = delivery.Distribution.ApplyDeliveryFeesWhen is DeliveryFeesApplication.Always ||
            //                             (delivery.Distribution.ApplyDeliveryFeesWhen is DeliveryFeesApplication
            //                                  .TotalLowerThanPurchaseOrderAmount &&
            //                              delivery.Distribution.DeliveryFeesMinPurchaseOrdersAmount.HasValue &&
            //                              delivery.Distribution.DeliveryFeesMinPurchaseOrdersAmount.Value >
            //                              TotalWholeSalePrice);
        }

        public Guid Id { get; } = Guid.NewGuid();
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset UpdatedOn { get; private set; }
        public bool Removed { get; private set; }
        public DateTimeOffset? AcceptedOn { get; private set; }
        public DateTimeOffset? CompletedOn { get; private set; }
        public DateTimeOffset? DroppedOn { get; private set; }
        public string Reference
        {
            get { return _identifier.Replace($"{Vendor.Id:N}_", string.Empty);}
            private set
            {
                _identifier = $"{Vendor.Id:N}_{value}";
            } 
        }
        public string Comment { get; private set; }
        public string Reason { get; private set; }
        public PurchaseOrderStatus Status { get; private set; }
        public Guid CartId { get; private set; }
        public Sender Sender { get; private set; }
        public Vendor Vendor { get; private set; }
        public PurchaseOrderDelivery ExpectedDelivery { get; private set; }
        public ICollection<PurchaseOrderLine> Lines { get; private set; }
        public List<DomainEvent> DomainEvents { get; } = new List<DomainEvent>();
        
        public void Restore()
        {
            Removed = false;
        }

        internal void SetStatus(PurchaseOrderStatus newStatus, bool skipNotification)
        {
            switch (newStatus)
            {
                case PurchaseOrderStatus.Accepted:
                    if (Status is PurchaseOrderStatus.Processing or PurchaseOrderStatus.Accepted)
                        break;
                    
                    if (Status != PurchaseOrderStatus.Pending)
                        throw new ValidationException("Impossible d'accepter la commande, elle n'est plus en attente.");

                    if (CreatedOn.AddDays(3) < DateTimeOffset.UtcNow)
                        throw new ValidationException("Impossible d'accepter la commande, le délai de 72h est écoulé.");

                    AcceptedOn = DateTimeOffset.UtcNow;

                    if (!skipNotification)
                        DomainEvents.Add(new PurchaseOrderAcceptedEvent(Id));

                    break;
                case PurchaseOrderStatus.Completed:
                    if (Status != PurchaseOrderStatus.Processing)
                        throw new ValidationException("La commande doit être en préparation pour pouvoir être complétée.");

                    CompletedOn ??= DateTimeOffset.UtcNow;

                    if (!skipNotification)
                        DomainEvents.Add(new PurchaseOrderCompletedEvent(Id));
                    break;
                case PurchaseOrderStatus.Delivered:
                    if (Status != PurchaseOrderStatus.Completed)
                        throw new ValidationException("La commande doit être prête pour pouvoir être livrée.");
                    break;
                case PurchaseOrderStatus.Cancelled:
                    if (Status == PurchaseOrderStatus.Cancelled)
                        throw new ValidationException("La commande a déjà été annulée.");
                    if (Status == PurchaseOrderStatus.Refused)
                        throw new ValidationException("La commande a déjà été refusée.");
                    if (Status == PurchaseOrderStatus.Delivered)
                        throw new ValidationException("La commande a déjà été livrée.");
                    if (Status == PurchaseOrderStatus.Expired)
                        throw new ValidationException("La commande est expirée.");

                    DroppedOn = DateTimeOffset.UtcNow;

                    if (!skipNotification)
                        DomainEvents.Add(new PurchaseOrderCancelledEvent(Id));
                    break;
                case PurchaseOrderStatus.Withdrawned:
                    if (Status == PurchaseOrderStatus.Withdrawned)
                        throw new ValidationException("La commande a déjà été annulée.");
                    if (Status == PurchaseOrderStatus.Refused)
                        throw new ValidationException("La commande a déjà été refusée.");
                    if (Status == PurchaseOrderStatus.Delivered)
                        throw new ValidationException("La commande a déjà été livrée.");
                    if (Status == PurchaseOrderStatus.Expired)
                        throw new ValidationException("La commande est expirée.");

                    DroppedOn = DateTimeOffset.UtcNow;

                    if (!skipNotification)
                        DomainEvents.Add(new PurchaseOrderWithdrawnedEvent(Id));
                    break;
                case PurchaseOrderStatus.Refused:
                    if (Status == PurchaseOrderStatus.Cancelled)
                        throw new ValidationException("La commande a déjà été annulée.");
                    if (Status == PurchaseOrderStatus.Refused)
                        throw new ValidationException("La commande a déjà été refusée.");
                    if (Status == PurchaseOrderStatus.Delivered)
                        throw new ValidationException("La commande a déjà été livrée.");
                    if (Status == PurchaseOrderStatus.Expired)
                        throw new ValidationException("La commande est expirée.");

                    DroppedOn = DateTimeOffset.UtcNow;

                    if (!skipNotification)
                        DomainEvents.Add(new PurchaseOrderRefusedEvent(Id));
                    break;
                case PurchaseOrderStatus.Expired:
                    if (Status != PurchaseOrderStatus.Pending)
                        throw new ValidationException("La commande ne peut pas être expirée, elle doit être en attente.");

                    DroppedOn = DateTimeOffset.UtcNow;

                    if (!skipNotification)
                        DomainEvents.Add(new PurchaseOrderExpiredEvent(Id));
                    break;
            }

            Status = newStatus;
        }

        public void Cancel(string reason, bool skipNotification)
        {            
            SetStatus(PurchaseOrderStatus.Cancelled, skipNotification);
            Reason = reason;
        }

        public void Withdrawn(string reason, bool skipNotification)
        {
            SetStatus(PurchaseOrderStatus.Withdrawned, skipNotification);
            Reason = reason;
        }

        public void Expire(string reason, bool skipNotification)
        {
            SetStatus(PurchaseOrderStatus.Expired, skipNotification);
            Reason = reason;
        }

        public void Refuse(string reason, bool skipNotification)
        {
            SetStatus(PurchaseOrderStatus.Refused, skipNotification);
            Reason = reason;
        }

        public void Process(bool skipNotification)
        {
            SetStatus(PurchaseOrderStatus.Processing, skipNotification);
        }

        public void Complete(bool skipNotification)
        {
            SetStatus(PurchaseOrderStatus.Completed, skipNotification);
        }

        public void Accept(bool skipNotification)
        {
            SetStatus(PurchaseOrderStatus.Accepted, skipNotification);
        }

        private void AddProduct(Tuple<Product, CatalogProductPrice, int> productWithPriceAndQuantity)
        {
            if (Status != PurchaseOrderStatus.Pending)
                throw new ValidationException("Impossible d'ajouter un produit, la commande n'est plus en attente.");

            var productLine = Lines.SingleOrDefault(p => p.ProductId == productWithPriceAndQuantity.Item1.Id);
            if (productLine != null)
                Lines.Remove(productLine);

            Lines.Add(new PurchaseOrderLine(productWithPriceAndQuantity.Item1, productWithPriceAndQuantity.Item2, productWithPriceAndQuantity.Item3));
        }
    }
}