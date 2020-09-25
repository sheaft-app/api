using Sheaft.Domain.Enums;
using Sheaft.Exceptions;
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

        public bool SkipBackgroundProcessing { get; private set; }
        public virtual PurchaseOrder PurchaseOrder { get; private set; }
        public virtual Wallet CreditedWallet { get; private set; }
        public virtual Wallet DebitedWallet { get; private set; }
        public virtual TransferRefund Refund { get; private set; }
        public virtual Payout Payout { get; private set; }

        public void SetRefund(TransferRefund refund)
        {
            if (Refund != null && Refund.Status == TransactionStatus.Succeeded)
                throw new ValidationException();
            
            Refund = null;
            Refund = refund;
        }

        public void SetSkipBackgroundProcessing(bool value)
        {
            SkipBackgroundProcessing = value;
        }
    }
}