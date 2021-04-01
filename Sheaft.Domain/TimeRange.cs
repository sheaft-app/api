using System;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public abstract class TimeRange : IIdEntity, ITrackCreation, ITrackUpdate
    {
        protected TimeRange()
        {
        }

        protected TimeRange(Guid id, DateTimeOffset from, DateTimeOffset to)
        {
            Id = id;
            ClosedFrom = from;
            ClosedTo = to;
        }

        public Guid Id { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public DateTimeOffset ClosedFrom { get; private set; }
        public DateTimeOffset ClosedTo { get; private set; }

        public void ChangeClosedDates(DateTimeOffset from, DateTimeOffset to)
        {
            ClosedFrom = from;
            ClosedTo = to;
        }
    }
}