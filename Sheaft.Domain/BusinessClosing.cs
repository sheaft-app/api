using System;

namespace Sheaft.Domain
{
    public class BusinessClosing : TimeRange
    {
        protected BusinessClosing()
        {
        }

        public BusinessClosing(Guid id, DateTimeOffset from, DateTimeOffset to, string reason = null)
            : base(id, from, to)
        {
            Reason = reason;
        }
        
        public string Reason { get; private set; }

        public void SetReason(string reason)
        {
            if (reason == null)
                return;

            Reason = reason;
        }
    }
}