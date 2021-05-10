using System;

namespace Sheaft.Domain
{
    public class BusinessClosing : TimeRange
    {
        protected BusinessClosing()
        {
        }

        public BusinessClosing(Business business, Guid id, DateTimeOffset from, DateTimeOffset to, string reason = null)
            : base(id, from, to)
        {
            Reason = reason;
            BusinessId = business.Id;
        }

        public Guid BusinessId { get; private set; }
        public string Reason { get; private set; }
        public byte[] RowVersion { get; private set; }

        public void SetReason(string reason)
        {
            if (reason == null)
                return;

            Reason = reason;
        }
    }
}