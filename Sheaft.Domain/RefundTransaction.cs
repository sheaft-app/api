using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Domain.Models
{
    public abstract class RefundTransaction : Transaction
    {
        protected RefundTransaction() { }

        protected RefundTransaction(Guid id, TransactionKind kind, PayinTransaction transaction, User author)
            : this(id, kind, transaction.Credited, transaction.CreditedWallet, author)
        {
        }

        protected RefundTransaction(Guid id, TransactionKind kind, TransferTransaction transaction, User author)
            : this(id, kind, transaction.Credited, transaction.CreditedWallet, author)
        {
        }

        protected RefundTransaction(Guid id, TransactionKind kind, decimal debited, Wallet debitedWallet, User author)
            : base(id, kind, author)
        {
            DebitedWallet = debitedWallet;
            Debited = debited;
        }

        public virtual Wallet DebitedWallet { get; private set; }
    }
}