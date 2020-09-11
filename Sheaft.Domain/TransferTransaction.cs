using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Domain.Models
{
    public class TransferTransaction : Transaction
    {
        protected TransferTransaction()
        {
        }

        public TransferTransaction(Guid id, Wallet debitedWallet, Wallet creditedWallet, PurchaseOrder purchaseOrder)
            : base(id, TransactionKind.Transfer, TransactionNature.Regular, debitedWallet.User)
        {
            PurchaseOrder = purchaseOrder;
            Debited = PurchaseOrder.TotalWholeSalePrice;
            Credited = PurchaseOrder.TotalWholeSalePrice;
            CreditedWallet = creditedWallet;
            DebitedWallet = debitedWallet;
            Reference = $"SHEAFT_{purchaseOrder.Reference}_{DateTimeOffset.UtcNow:yyyyMMdd}";
        }

        public virtual PurchaseOrder PurchaseOrder { get; private set; }
    }
}