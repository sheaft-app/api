using System;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain.Enum;

namespace Sheaft.Domain
{
    public abstract class TimeSlotHour
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

        public Guid Id { get; set; }
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