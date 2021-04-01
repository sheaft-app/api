using System;
using System.Collections.Generic;
using System.Linq;

namespace Sheaft.Domain
{
    public class DeliveryClosing : TimeRange
    {
        protected DeliveryClosing()
        {
        }

        public DeliveryClosing(Guid id, DateTimeOffset from, DateTimeOffset to, string reason = null)
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