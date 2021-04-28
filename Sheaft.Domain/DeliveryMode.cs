using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Core.Exceptions;
using Sheaft.Domain.Common;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class DeliveryMode : IEntity, IHasDomainEvent
    {
        private List<DeliveryHours> _deliveryHours;
        private List<DeliveryClosing> _closings;

        protected DeliveryMode()
        {
        }

        public DeliveryMode(Guid id, DeliveryKind kind, Producer producer, bool available, DeliveryAddress address,
            IEnumerable<DeliveryHours> openingHours, string name, string description = null)
        {
            Id = id;
            Name = name;
            Kind = kind;
            Description = description;

            Address = address;
            Producer = producer;
            ProducerId = producer.Id;

            SetDeliveryHours(openingHours);
            SetAvailability(available);
            DomainEvents = new List<DomainEvent>();
        }

        public Guid Id { get; private set; }
        public DeliveryKind Kind { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public DateTimeOffset? RemovedOn { get; private set; }
        public int? LockOrderHoursBeforeDelivery { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int? MaxPurchaseOrdersPerTimeSlot { get; private set; }
        public bool Available { get; private set; }
        public bool AutoAcceptRelatedPurchaseOrder { get; private set; }
        public bool AutoCompleteRelatedPurchaseOrder { get; private set; }
        public virtual DeliveryAddress Address { get; private set; }
        public Guid ProducerId { get; private set; }
        public virtual Producer Producer { get; private set; }
        public virtual IReadOnlyCollection<DeliveryHours> DeliveryHours => _deliveryHours?.AsReadOnly();
        public virtual IReadOnlyCollection<DeliveryClosing> Closings => _closings?.AsReadOnly();

        public void SetDeliveryHours(IEnumerable<DeliveryHours> deliveryHours)
        {
            _deliveryHours = deliveryHours.ToList();
        }

        public void SetLockOrderHoursBeforeDelivery(int? lockOrderHoursBeforeDelivery)
        {
            LockOrderHoursBeforeDelivery = lockOrderHoursBeforeDelivery;
        }

        public void SetAddress(string line1, string line2, string zipcode, string city, CountryIsoCode country,
            double? longitude = null, double? latitude = null)
        {
            Address = new DeliveryAddress(line1, line2, zipcode, city, country, longitude, latitude);
        }

        public void SetDescription(string description)
        {
            Description = description;
        }

        public void SetAvailability(bool available)
        {
            Available = available;
        }

        public void SetAutoAcceptRelatedPurchaseOrders(bool autoAccept)
        {
            AutoAcceptRelatedPurchaseOrder = autoAccept;
        }

        public void SetAutoCompleteRelatedPurchaseOrders(bool autoComplete)
        {
            AutoCompleteRelatedPurchaseOrder = autoComplete;
        }

        public void SetName(string name)
        {
            Name = name;
        }

        public void SetKind(DeliveryKind kind)
        {
            Kind = kind;
        }

        public void SetMaxPurchaseOrdersPerTimeSlot(int? maxPurchaseOrdersPerTimeSlot)
        {
            MaxPurchaseOrdersPerTimeSlot = maxPurchaseOrdersPerTimeSlot;
        }

        public DeliveryClosing AddClosing(DateTimeOffset from, DateTimeOffset to, string reason = null)
        {
            if (Closings == null)
                _closings = new List<DeliveryClosing>();

            var closing = new DeliveryClosing(Guid.NewGuid(), from, to, reason);
            _closings.Add(closing);

            return closing;
        }

        public void RemoveClosings(IEnumerable<Guid> ids)
        {
            foreach (var id in ids)
                RemoveClosing(id);
        }

        public void RemoveClosing(Guid id)
        {
            var closing = _closings.SingleOrDefault(r => r.Id == id);
            if (closing == null)
                throw SheaftException.NotFound();

            _closings.Remove(closing);
        }

        public List<DomainEvent> DomainEvents { get; } = new List<DomainEvent>();
    }
}