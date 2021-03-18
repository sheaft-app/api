using System;
using System.Linq;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain.Enum;

namespace Sheaft.Domain
{
    public abstract class ExpectedDelivery
    {
        protected ExpectedDelivery()
        {
        }

        public ExpectedDelivery(DeliveryMode mode, DateTimeOffset expectedDeliveryDate)
        {
            SetExpectedDate(expectedDeliveryDate);

            var oh = GetOpeningHour(mode, expectedDeliveryDate);
            From = oh.From;
            To = oh.To;        }

        public DateTimeOffset ExpectedDeliveryDate { get; private set; }
        public TimeSpan From { get; private set; }
        public TimeSpan To { get; private set; }

        public void SetExpectedDate(DateTimeOffset date)
        {
            if (date < DateTimeOffset.UtcNow)
                throw new ValidationException(MessageKind.ExpectedDelivery_ExpectedDate_CannotBe_BeforeNow, date.ToString("dd/MM/yyyy"));

            ExpectedDeliveryDate = date;
        }

        protected TimeSlotHour GetOpeningHour(DeliveryMode delivery, DateTimeOffset expectedDeliveryDate)
        {
            if (delivery.RemovedOn.HasValue)
                throw new ValidationException(MessageKind.ExpectedDelivery_ExpectedDate_DeliveryRemoved, delivery.Name, expectedDeliveryDate.ToString("dd/MM/yyyy"));

            var oh = delivery.OpeningHours.FirstOrDefault(o => o.Day == expectedDeliveryDate.DayOfWeek && o.From <= expectedDeliveryDate.TimeOfDay && o.To >= expectedDeliveryDate.TimeOfDay);
            if (oh == null)
                throw new ValidationException(MessageKind.ExpectedDelivery_ExpectedDate_NotIn_DeliveryOpeningHours, delivery.Name, expectedDeliveryDate.ToString("dd/MM/yyyy"));

            if (delivery.LockOrderHoursBeforeDelivery.HasValue && expectedDeliveryDate.AddHours(-delivery.LockOrderHoursBeforeDelivery.Value) < DateTime.UtcNow)
                throw new ValidationException(MessageKind.ExpectedDelivery_ExpectedDate_OrdersLocked, delivery.Name, expectedDeliveryDate.ToString("dd/MM/yyyy"));

            return oh;
        }
    }
}