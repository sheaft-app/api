using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Exceptions;
using Sheaft.Interop;
using Sheaft.Interop.Enums;

namespace Sheaft.Domain.Models
{
    public class DeliveryMode : IEntity
    {
        private List<TimeSlotHour> _openingHours;

        protected DeliveryMode()
        {
        }

        public DeliveryMode(Guid id, DeliveryKind kind, Producer producer, int lockOrderHoursBeforeDelivery, LocationAddress address, IEnumerable<TimeSlotHour> openingHours, string name, string description = null)
        {
            Id = id;
            Name = name;
            Kind = kind;
            Description = description;
            LockOrderHoursBeforeDelivery = lockOrderHoursBeforeDelivery;

            Address = address;
            Producer = producer;

            SetOpeningHours(openingHours);
        }

        public Guid Id { get; private set; }
        public DeliveryKind Kind { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public DateTimeOffset? RemovedOn { get; private set; }
        public int LockOrderHoursBeforeDelivery { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public virtual LocationAddress Address { get; private set; }
        public virtual Producer Producer { get; private set; }
        public virtual IReadOnlyCollection<TimeSlotHour> OpeningHours { get { return _openingHours.AsReadOnly(); } }

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
            Address = new LocationAddress(line1, line2, zipcode, city, country, longitude, latitude);
        }

        public void SetDescription(string description)
        {
            Description = description;
        }

        public void SetName(string name)
        {
            Name = name;
        }

        public void SetKind(DeliveryKind kind)
        {
            Kind = kind;
        }

        public void Remove()
        {
        }

        public void Restore()
        {
            RemovedOn = null;
        }
    }
}