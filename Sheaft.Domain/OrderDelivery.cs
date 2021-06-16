using System;
using System.Linq;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class OrderDelivery : IIdEntity, ITrackCreation, ITrackUpdate
    {
        protected OrderDelivery()
        {
        }

        public OrderDelivery(DeliveryMode delivery, DateTimeOffset expectedDeliveryDate, string comment = null)
        {
            Id = Guid.NewGuid();
            DeliveryMode = delivery;
            DeliveryModeId = delivery.Id;
            Comment = comment;
            
            SetExpectedDate(expectedDeliveryDate);

            var oh = GetOpeningHour(delivery, expectedDeliveryDate);
            From = oh.From;
            To = oh.To;
        }

        public Guid Id { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public DateTimeOffset ExpectedDeliveryDate { get; private set; }
        public DayOfWeek Day { get; private set; }
        public TimeSpan From { get; private set; }
        public TimeSpan To { get; private set; }
        public string Comment { get; private set; }
        public Guid OrderId { get; private set; }
        public Guid DeliveryModeId { get; private set; }
        public virtual DeliveryMode DeliveryMode { get; private set; }
        public byte[] RowVersion { get; private set; }

        public void SetExpectedDate(DateTimeOffset date)
        {
            if (date < DateTimeOffset.UtcNow)
                throw new ValidationException(MessageKind.ExpectedDelivery_ExpectedDate_CannotBe_BeforeNow,
                    date.ToString("dd/MM/yyyy"));

            ExpectedDeliveryDate = date;
            Day = ExpectedDeliveryDate.DayOfWeek;
        }

        protected TimeSlotHour GetOpeningHour(DeliveryMode delivery, DateTimeOffset expectedDeliveryDate)
        {
            if (delivery.RemovedOn.HasValue)
                throw new ValidationException(MessageKind.ExpectedDelivery_ExpectedDate_DeliveryRemoved, delivery.Name,
                    expectedDeliveryDate.ToString("dd/MM/yyyy"));

            var oh = delivery.DeliveryHours.FirstOrDefault(o =>
                o.Day == expectedDeliveryDate.DayOfWeek && o.From <= expectedDeliveryDate.TimeOfDay &&
                o.To >= expectedDeliveryDate.TimeOfDay);
            if (oh == null)
                throw new ValidationException(MessageKind.ExpectedDelivery_ExpectedDate_NotIn_DeliveryOpeningHours,
                    delivery.Name, expectedDeliveryDate.ToString("dd/MM/yyyy"));

            if (delivery.LockOrderHoursBeforeDelivery.HasValue &&
                expectedDeliveryDate.AddHours(-delivery.LockOrderHoursBeforeDelivery.Value) < DateTime.UtcNow)
                throw new ValidationException(MessageKind.ExpectedDelivery_ExpectedDate_OrdersLocked, delivery.Name,
                    expectedDeliveryDate.ToString("dd/MM/yyyy"));

            return oh;
        }
    }
}