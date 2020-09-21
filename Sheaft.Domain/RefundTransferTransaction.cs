using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Domain.Models
{
    public class RefundTransferTransaction : RefundTransaction
    {
        protected RefundTransferTransaction()
        {
        }

        public RefundTransferTransaction(Guid id, TransferTransaction transaction, User author)
            : base(id, TransactionKind.RefundTransfer, transaction, author)
        {
            CreditedWallet = transaction.DebitedWallet;
            Credited = transaction.Debited;
            TransferTransaction = transaction;
        }

        public virtual Wallet CreditedWallet { get; private set; }
        public virtual TransferTransaction TransferTransaction { get; private set; }
    }
}