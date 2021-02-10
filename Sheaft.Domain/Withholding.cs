using Sheaft.Domain.Enums;
using System;

namespace Sheaft.Domain.Models
{
    public class Withholding : Transaction
    {
        protected Withholding()
        {
        }

        public Withholding(Guid id, decimal amount, Wallet debitedWallet, Wallet creditedWallet)
            : base(id, TransactionKind.Transfer, debitedWallet.User)
        {
            Fees = 0;
            Debited = amount;
            CreditedWallet = creditedWallet;
            DebitedWallet = debitedWallet;
            Credited = Debited;
            Reference = "WITHHOLDING";
        }

        public virtual Wallet CreditedWallet { get; private set; }
        public virtual Wallet DebitedWallet { get; private set; }
        public virtual Payout Payout { get; private set; }
    }
}