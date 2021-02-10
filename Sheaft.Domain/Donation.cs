using Sheaft.Domain.Enums;
using System;

namespace Sheaft.Domain.Models
{
    public class Donation : Transaction
    {
        protected Donation()
        {
        }

        public Donation(Guid id, Wallet debitedWallet, Wallet creditedWallet, Order order)
            : base(id, TransactionKind.Transfer, debitedWallet.User)
        {
            Order = order;
            Fees = 0;
            Debited = Math.Round(order.Donate - order.InternalFeesPrice, 2);
            CreditedWallet = creditedWallet;
            DebitedWallet = debitedWallet;
            Credited = Debited;
            Reference = "DONATION";
        }

        public virtual Wallet CreditedWallet { get; private set; }
        public virtual Wallet DebitedWallet { get; private set; }
        public virtual Order Order { get; private set; }
    }
}