using Sheaft.Domain.Enums;
using System;

namespace Sheaft.Domain.Models
{
    public class PayinRefund : Refund
    {
        protected PayinRefund()
        {
        }

        public PayinRefund(Guid id, Payin transaction, User author)
            : base(id, TransactionKind.RefundPayin, transaction, author)
        {
            Credited = transaction.Debited;
            Payin = transaction;
        }

        public virtual Payin Payin { get; private set; }
    }
}