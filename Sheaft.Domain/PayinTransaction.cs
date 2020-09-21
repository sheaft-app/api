using Sheaft.Domain.Enums;
using System;

namespace Sheaft.Domain.Models
{
    public abstract class PayinTransaction : Transaction
    {
        protected PayinTransaction()
        {
        }

        protected PayinTransaction(Guid id, TransactionKind kind, Wallet creditedWallet, Order order)
            : base(id, kind, creditedWallet.User)
        {
            Order = order;
            Fees = order.FeesPrice + order.Donation;
            Debited = order.TotalWholeSalePrice + order.FeesPrice + order.Donation;
            CreditedWallet = creditedWallet;
            Reference = "SHEAFT";
        }

        public virtual Wallet CreditedWallet { get; private set; }
        public virtual Order Order { get; private set; }
    }
}