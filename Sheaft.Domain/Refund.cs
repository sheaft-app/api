using Sheaft.Domain.Enums;
using System;

namespace Sheaft.Domain.Models
{
    public abstract class Refund : Transaction
    {
        protected Refund() { }

        protected Refund(Guid id, TransactionKind kind, Payin transaction)
            : this(id, kind, transaction.Credited, transaction.CreditedWallet, transaction.Author)
        {
        }

        protected Refund(Guid id, TransactionKind kind, Transfer transaction)
            : this(id, kind, transaction.Credited, transaction.CreditedWallet, transaction.Author)
        {
        }

        protected Refund(Guid id, TransactionKind kind, decimal debited, Wallet debitedWallet, User author)
            : base(id, kind, author)
        {
            DebitedWallet = debitedWallet;
            Debited = debited;
        }

        public virtual Wallet DebitedWallet { get; private set; }
    }
}