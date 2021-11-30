using System;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain.BaseClass
{
    public abstract class TimeRange : IIdEntity, ITrackCreation, ITrackUpdate
    {
        protected TimeRange()
        {
        }

        protected TimeRange(DateTimeOffset from, DateTimeOffset to)
        {
            Id = Guid.NewGuid();
            ClosedFrom = GetFromDate(@from.LocalDateTime);
            ClosedTo = GetToDate(@from.LocalDateTime, to.LocalDateTime);
        }

        public Guid Id { get; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset UpdatedOn { get; private set; }
        public DateTimeOffset ClosedFrom { get; private set; }
        public DateTimeOffset ClosedTo { get; private set; }

        public void ChangeClosedDates(DateTimeOffset from, DateTimeOffset to)
        {
            ClosedFrom = GetFromDate(@from.LocalDateTime);
            ClosedTo = GetToDate(@from.LocalDateTime, to.LocalDateTime);
        }

        private static DateTime GetToDate(DateTimeOffset @from, DateTimeOffset to)
        {
            return @from.Date == to.Date ? new DateTime(to.Year, to.Month, to.Day, 23, 59, 59) : new DateTime(to.Year, to.Month, to.Day, 0, 0, 0);
        }

        private static DateTime GetFromDate(DateTimeOffset @from)
        {
            return new DateTime(@from.Year, @from.Month, @from.Day, 0, 0, 0);
        }
    }
}