using Sheaft.Domain.Enums;
using System;

namespace Sheaft.Domain.Models
{
    public class TransferTransaction : Transaction
    {
        protected TransferTransaction()
        {
        }

        public TransferTransaction(Guid id, Wallet debitedWallet, Wallet creditedWallet, PurchaseOrder purchaseOrder)
            : base(id, TransactionKind.Transfer, debitedWallet.User)
        {
            PurchaseOrder = purchaseOrder;
            Debited = PurchaseOrder.TotalWholeSalePrice;
            Credited = PurchaseOrder.TotalWholeSalePrice;
            CreditedWallet = creditedWallet;
            DebitedWallet = debitedWallet;
            Reference = "SHEAFT";
        }

        public virtual PurchaseOrder PurchaseOrder { get; private set; }
        public virtual PayoutTransaction Payout { get; private set; }
        public virtual Wallet CreditedWallet { get; private set; }
        public virtual Wallet DebitedWallet { get; private set; }
    }
}