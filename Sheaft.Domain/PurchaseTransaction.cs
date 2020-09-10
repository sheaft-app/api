using System;

namespace Sheaft.Domain.Models
{
    public class PurchaseTransaction : Transaction
    {
        protected PurchaseTransaction()
        {
        }

        public PurchaseTransaction(Guid id)
            : base(id)
        {
        }
    }
}