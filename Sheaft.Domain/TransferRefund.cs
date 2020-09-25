using Sheaft.Domain.Enums;
using System;

namespace Sheaft.Domain.Models
{
    public class TransferRefund : Refund
    {
        protected TransferRefund()
        {
        }

        public TransferRefund(Guid id, Transfer transaction)
            : base(id, TransactionKind.RefundTransfer, transaction)
        {
            CreditedWallet = transaction.DebitedWallet;
            Credited = transaction.Debited;
            Transfer = transaction;
        }

        public virtual Wallet CreditedWallet { get; private set; }
        public virtual Transfer Transfer { get; private set; }
    }
}