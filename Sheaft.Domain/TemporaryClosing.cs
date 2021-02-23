using System;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public abstract class TemporaryClosing : IEntity
    {
        protected TemporaryClosing()
        {
        }

        protected TemporaryClosing(Guid id, DateTimeOffset from, DateTimeOffset to, string reason = null)
        {
            Id = id;
            ClosedFrom = from;
            ClosedTo = to;
            Reason = reason;
        }

        public Guid Id { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public DateTimeOffset? RemovedOn { get; private set; }
        public DateTimeOffset ClosedFrom { get; private set; }
        public DateTimeOffset ClosedTo { get; private set; }
        public string Reason { get; private set; }

        public void ChangeClosedDates(DateTimeOffset from, DateTimeOffset to)
        {
            ClosedFrom = from;
            ClosedTo = to;
        }
    }
}