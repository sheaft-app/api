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
                throw new ValidationException(MessageKind.TimeSlot_From_CannotBe_GreaterOrEqualThan, 24);

            if (to > TimeSpan.FromDays(1))
                throw new ValidationException(MessageKind.TimeSlot_To_CannotBe_GreaterOrEqualThan, 24);

            if (from >= to)
                throw new ValidationException(MessageKind.TimeSlot_From_CannotBe_GreaterOrEqualThan_To);

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