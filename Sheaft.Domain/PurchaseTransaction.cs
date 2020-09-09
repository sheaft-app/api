using System;
using Sheaft.Interop;
using Sheaft.Interop.Enums;

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