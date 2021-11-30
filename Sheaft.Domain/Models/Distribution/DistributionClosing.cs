using System;
using Sheaft.Domain.BaseClass;

namespace Sheaft.Domain
{
    public class DistributionClosing : TimeRange
    {
        protected DistributionClosing()
        {
        }

        public DistributionClosing(DateTimeOffset from, DateTimeOffset to, string reason = null)
            : base(from, to)
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