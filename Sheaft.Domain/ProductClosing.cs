using System;

namespace Sheaft.Domain
{
    public class ProductClosing : TemporaryClosing
    {
        protected ProductClosing()
        {
        }

        public ProductClosing(Guid id, DateTimeOffset from, DateTimeOffset to, string reason = null)
            : base(id, from, to, reason)
        {
        }
    }
}