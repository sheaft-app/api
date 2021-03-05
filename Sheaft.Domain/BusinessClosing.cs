using System;

namespace Sheaft.Domain
{
    public class BusinessClosing : TemporaryClosing
    {
        protected BusinessClosing()
        {
        }
        
        public BusinessClosing(Guid id, DateTimeOffset from, DateTimeOffset to, string reason = null)
            :base(id, from, to, reason)
        {
        }
    }
}