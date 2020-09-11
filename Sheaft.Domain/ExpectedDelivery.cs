using Sheaft.Exceptions;
using Sheaft.Interop.Enums;
using System;
using System.Linq;

namespace Sheaft.Domain.Models
{
    public class ExpectedDelivery
    {
        protected ExpectedDelivery()
        {
        }

        public ExpectedDelivery(DeliveryMode mode, DateTimeOffset expectedDeliveryDate)
        {
            SetExpectedDate(expectedDeliveryDate);

            var oh = GetOpeningHour(mode, expectedDeliveryDate);
            From = oh.From;
            To = oh.To;

            Kind = mode.Kind;
            Name = mode.Name;
            Address = new ExpectedAddress(mode.Address.Line1, mode.Address.Line2, mode.Address.Zipcode, mode.Address.City, mode.Address.Country, mode.Address.Longitude, mode.Address.Latitude);
        }

        public DeliveryKind Kind { get; private set; }
        public string Name { get; private set; }
        public DateTimeOffset ExpectedDeliveryDate { get; private set; }
        public DateTimeOffset? DeliveryStartedOn { get; private set; }
        public DateTimeOffset? DeliveredOn { get; private set; }
        public TimeSpan From { get; private set; }
        public TimeSpan To { get; private set; }
        public virtual ExpectedAddress Address { get; private set; }

        public void SetExpectedDate(DateTimeOffset date)
        {
            if (date < DateTimeOffset.UtcNow)
                throw new ValidationException(MessageKind.ExpectedDelivery_ExpectedDate_CannotBe_BeforeNow);

            ExpectedDeliveryDate = date;
        }

        public void SetDeliveredDate(DateTimeOffset date)
        {
            DeliveredOn = date;
        }

        private TimeSlotHour GetOpeningHour(DeliveryMode delivery, DateTimeOffset expectedDeliveryDate)
        {
            var oh = delivery.OpeningHours.FirstOrDefault(o => o.Day == expectedDeliveryDate.DayOfWeek && o.From <= expectedDeliveryDate.TimeOfDay && o.To >= expectedDeliveryDate.TimeOfDay);
            if(oh == null)
                throw new ValidationException(MessageKind.ExpectedDelivery_ExpectedDate_NotIn_DeliveryOpeningHours);

            return oh;
        }
    }
}