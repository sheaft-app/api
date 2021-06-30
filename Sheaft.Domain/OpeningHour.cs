using System;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public abstract class TimeSlotHour : IIdEntity, ITrackCreation, ITrackUpdate
    {
        protected TimeSlotHour()
        {
        }

        protected TimeSlotHour(DayOfWeek day, TimeSpan from, TimeSpan to)
        {
            if (from >= TimeSpan.FromDays(1))
                throw SheaftException.Validation("L'heure de début ne peut pas être supérieure à 24 heures.");

            if (to > TimeSpan.FromDays(1))
                throw SheaftException.Validation("L'heure de fin ne peut pas être supérieure à 24 heures.");

            if (from >= to)
                throw SheaftException.Validation("L'heure de début ne peut pas être supérieur à l'heure de fin.");

            Day = day;
            From = from;
            To = to;
        }

        public Guid Id { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public DayOfWeek Day { get; private set; }
        public TimeSpan From { get; private set; }
        public TimeSpan To { get; private set; }
    }

    public class DeliveryHours : TimeSlotHour
    {
        public DeliveryHours(DayOfWeek day, TimeSpan from, TimeSpan to)
            :base(day, from, to)
        {
        }
        
        public Guid DeliveryModeId { get; set; }
    }

    public class OpeningHours : TimeSlotHour
    {
        public OpeningHours(DayOfWeek day, TimeSpan from, TimeSpan to)
            :base(day, from, to)
        {
        }
        
        public Guid StoreId { get; set; }
    }
}