using System;
using System.Collections.Generic;
using System.Linq;

namespace Sheaft.Domain
{
    public class DeliveryClosing : TemporaryClosing
    {
        protected DeliveryClosing()
        {
        }

        public DeliveryClosing(Guid id, DateTimeOffset from, DateTimeOffset to, string reason = null)
            : base(id, from, to, reason)
        {
        }
    }
}