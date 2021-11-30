using System;
using Sheaft.Domain.BaseClass;

namespace Sheaft.Domain
{
    public class CompanyClosing : TimeRange
    {
        protected CompanyClosing()
        {
        }

        public CompanyClosing(Company company, DateTimeOffset from, DateTimeOffset to, string reason = null)
            : base(from, to)
        {
            Reason = reason;
            CompanyId = company.Id;
        }

        public Guid CompanyId { get; private set; }
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