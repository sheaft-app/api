using System;
using System.Linq;
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
                throw SheaftException.Validation("La date de livraison ne peut pas être inférieure à la date du jour.");

            ExpectedDeliveryDate = date;
            Day = ExpectedDeliveryDate.DayOfWeek;
        }

        protected TimeSlotHour GetOpeningHour(DeliveryMode delivery, DateTimeOffset expectedDeliveryDate)
        {
            if (delivery.RemovedOn.HasValue)
                throw SheaftException.Validation("Ce mode de livraison n'existe plus.");

            var oh = delivery.DeliveryHours.FirstOrDefault(o =>
                o.Day == expectedDeliveryDate.DayOfWeek && o.From <= expectedDeliveryDate.TimeOfDay &&
                o.To >= expectedDeliveryDate.TimeOfDay);
            if (oh == null)
                throw SheaftException.Validation("La date selectionnée ne correspond à aucune plage horaire du mode de livraison.");

            if (delivery.LockOrderHoursBeforeDelivery.HasValue &&
                expectedDeliveryDate.AddHours(-delivery.LockOrderHoursBeforeDelivery.Value) < DateTime.UtcNow)
                throw SheaftException.Validation($"La date selectionnée se situe après la limite de {delivery.LockOrderHoursBeforeDelivery.Value}h avant le début de la livraison.");

            return oh;
        }
    }
}