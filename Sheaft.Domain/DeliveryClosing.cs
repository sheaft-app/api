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

        public DeliveryClosing(DeliveryMode delivery, Guid id, DateTimeOffset from, DateTimeOffset to, string reason = null)
            : base(id, from, to)
        {
            Reason = reason;
            DeliveryModeId = delivery.Id;
        }

        public Guid DeliveryModeId { get; private set; }
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