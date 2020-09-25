using Sheaft.Domain.Enums;
using System;

namespace Sheaft.Domain.Models
{
    public class Transfer : Transaction
    {
        protected Transfer()
        {
        }

        public Transfer(Guid id, Wallet debitedWallet, Wallet creditedWallet, PurchaseOrder purchaseOrder)
            : base(id, TransactionKind.Transfer, debitedWallet.User)
        {
            PurchaseOrder = purchaseOrder;
            Debited = PurchaseOrder.TotalWholeSalePrice;
            Credited = PurchaseOrder.TotalWholeSalePrice;
            CreditedWallet = creditedWallet;
            DebitedWallet = debitedWallet;
            Reference = "SHEAFT";
        }

        public DateTimeOffset? RefundedOn { get; private set; }
        public DateTimeOffset? PayedOutOn { get; private set; }
        public virtual PurchaseOrder PurchaseOrder { get; private set; }
        public virtual Wallet CreditedWallet { get; private set; }
        public virtual Wallet DebitedWallet { get; private set; }
    }
}