using Sheaft.Domain.Enums;
using System;

namespace Sheaft.Domain.Models
{
    public class PayinRefund : Refund
    {
        protected PayinRefund()
        {
        }

        public PayinRefund(Guid id, Payin transaction, decimal amount)
            : base(id, TransactionKind.RefundPayin, amount, transaction.CreditedWallet, transaction.Author)
        {
            Credited = transaction.Debited;
            Payin = transaction;
        }

        public virtual Payin Payin { get; private set; }
    }
}