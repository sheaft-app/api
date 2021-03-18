using System;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain.Enum;

namespace Sheaft.Domain
{
    public class TimeSlotHour
    {
        protected TimeSlotHour()
        {
        }

        public TimeSlotHour(DayOfWeek day, TimeSpan from, TimeSpan to)
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

        public DayOfWeek Day { get; private set; }
        public TimeSpan From { get; private set; }
        public TimeSpan To { get; private set; }
    }
}