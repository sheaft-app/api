using System;
using Sheaft.Domain.Enum;

namespace Sheaft.Domain
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
            DebitedWalletId = debitedWallet.Id;
            Debited = debited;
        }

        public Guid DebitedWalletId { get; private set; }
        public virtual Wallet DebitedWallet { get; private set; }
    }
}