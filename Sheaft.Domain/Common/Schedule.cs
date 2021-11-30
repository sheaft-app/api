using System;
using Sheaft.Domain.Exceptions;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain.BaseClass
{
    public abstract class Schedule : ITrackUpdate
    {
        protected Schedule()
        {
        }

        protected Schedule(DayOfWeek day, TimeSpan from, TimeSpan to)
        {
            if (from >= TimeSpan.FromDays(1))
                throw new ValidationException("L'heure de début ne peut pas être supérieure à 24 heures.");

            if (to > TimeSpan.FromDays(1))
                throw new ValidationException("L'heure de fin ne peut pas être supérieure à 24 heures.");

            if (from >= to)
                throw new ValidationException("L'heure de début ne peut pas être supérieur à l'heure de fin.");

            Day = day;
            From = from;
            To = to;
        }

        public DateTimeOffset UpdatedOn { get; private set; }
        public DayOfWeek Day { get; private set; }
        public TimeSpan From { get; private set; }
        public TimeSpan To { get; private set; }
    }

    public class DistributionSchedule : Schedule
    {
        public DistributionSchedule(DayOfWeek day, TimeSpan from, TimeSpan to)
            :base(day, from, to)
        {
        }
    }

    public class DeliverySchedule : Schedule
    {
        public DeliverySchedule(DayOfWeek day, TimeSpan from, TimeSpan to)
            :base(day, from, to)
        {
        }
    }

    public class CompanySchedule : Schedule
    {
        public CompanySchedule(DayOfWeek day, TimeSpan from, TimeSpan to)
            :base(day, from, to)
        {
        }
    }
}