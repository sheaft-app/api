using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Domain.Common;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events;
using Sheaft.Domain.Events.Delivery;
using Sheaft.Domain.Exceptions;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class Delivery : IIdEntity, ITrackCreation, ITrackUpdate, ITrackRemove, IHasDomainEvent
    {
        protected Delivery()
        {
        }

        public Delivery(int reference, DistributionKind kind, DateTimeOffset scheduledOn,
            DeliveryAddress address, Guid supplierId, Guid clientId, IEnumerable<Guid> purchaseOrdersIds)
        {
            Id = Guid.NewGuid();
            Reference = reference;
            Status = DeliveryStatus.Pending;
            Kind = kind;
            ScheduledOn = scheduledOn;
            Address = address;
            ClientId = clientId;
            SupplierId = supplierId;
        }

        public Guid Id { get; }
        public int Reference { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset UpdatedOn { get; private set; }
        public bool Removed { get; private set; }
        public DateTimeOffset? BilledOn { get; private set; }
        public DistributionKind Kind { get; private set; }
        public DeliveryStatus Status { get; private set; }
        public DateTimeOffset ScheduledOn { get; private set; }
        public DateTimeOffset? CompletedOn { get; private set; }
        public string ReceptionedBy { get; private set; }
        public string Comment { get; private set; }
        public DeliveryAddress Address { get; private set; }
        public string InitialDeliveryFormUrl { get; private set; }
        public string CompletedDeliveryFormUrl { get; private set; }
        public string DeliveryReceiptUrl { get; private set; }
        public decimal DeliveryFeesWholeSalePrice { get; private set; }
        public DateTimeOffset? ReceiptSentOn { get; set; }
        public DateTimeOffset? DeliveryFormSentOn { get; set; }
        public Guid SupplierId { get; private set; }
        public Guid ClientId { get; private set; }
        public Company Supplier { get; set; }
        public Company Client { get; set; }
        public ICollection<PickingOrder> Pickings { get; private set; }
        public ICollection<DeliveryProduct> Products { get; private set; }
        public ICollection<DeliveryReturnable> PickedUpReturnables { get; private set; }
        public List<DomainEvent> DomainEvents { get; } = new List<DomainEvent>();
        
        public void Restore()
        {
            Removed = false;
        }

        public void SetAsBilled()
        {
            if (Status != DeliveryStatus.Completed)
                throw new ValidationException("La livraison n'a pas encore été validée.");

            BilledOn = DateTimeOffset.UtcNow;
        }

        public void SetFormUrl(string url)
        {
            InitialDeliveryFormUrl = url;
            CompletedDeliveryFormUrl = url;
            DomainEvents.Add(new DeliveryFormGeneratedEvent(Id));
        }

        public void SetReceiptUrl(string url)
        {
            DeliveryReceiptUrl = url;
            DomainEvents.Add(new DeliveryReceiptGeneratedEvent(Id));
        }
    }
}