using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Exceptions;
using Sheaft.Domain.Enums;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain.Models
{
    public class DeliveryMode : IEntity
    {
        private List<TimeSlotHour> _openingHours;

        protected DeliveryMode()
        {
        }

        public DeliveryMode(Guid id, DeliveryKind kind, Producer producer, bool available, int lockOrderHoursBeforeDelivery, DeliveryAddress address, IEnumerable<TimeSlotHour> openingHours, string name, string description = null)
        {
            Id = id;
            Name = name;
            Kind = kind;
            Description = description;
            LockOrderHoursBeforeDelivery = lockOrderHoursBeforeDelivery;

            Address = address;
            Producer = producer;

            SetOpeningHours(openingHours);
            SetAvailability(available);
        }

        public Guid Id { get; private set; }
        public DeliveryKind Kind { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public DateTimeOffset? RemovedOn { get; private set; }
        public int LockOrderHoursBeforeDelivery { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool Available { get; private set; }
        public bool AutoAcceptRelatedPurchaseOrder { get; private set; }
        public bool AutoCompleteRelatedPurchaseOrder { get; private set; }
        public virtual DeliveryAddress Address { get; private set; }
        public virtual Producer Producer { get; private set; }
        public virtual IReadOnlyCollection<TimeSlotHour> OpeningHours => _openingHours?.AsReadOnly(); 

        public void SetOpeningHours(IEnumerable<TimeSlotHour> openingHours)
        {
            _openingHours = openingHours.ToList();
        }

        public void SetLockOrderHoursBeforeDelivery(int lockOrderHoursBeforeDelivery)
        {
            if (lockOrderHoursBeforeDelivery < 0)
                throw new ValidationException(MessageKind.DeliveryMode_LockOrderHoursBeforeDelivery_CannotBe_LowerThan, 0);

            LockOrderHoursBeforeDelivery = lockOrderHoursBeforeDelivery;
        }

        public void SetAddress(string line1, string line2, string zipcode, string city, CountryIsoCode country, double? longitude = null, double? latitude = null)
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

        public void SetAvailability(object available)
        {
            throw new NotImplementedException();
        }
    }
}