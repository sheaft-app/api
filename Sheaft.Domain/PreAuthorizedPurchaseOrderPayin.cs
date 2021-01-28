using Sheaft.Domain.Enums;
using System;

namespace Sheaft.Domain.Models
{
    public class PreAuthorizedPurchaseOrderPayin : PreAuthorizedPayin
    {
        protected PreAuthorizedPurchaseOrderPayin()
        {
        }

        public PreAuthorizedPurchaseOrderPayin(Guid id, PreAuthorization preAuthorization, PurchaseOrder purchaseOrder, Wallet creditedWallet)
            : base(id, TransactionKind.PreAuthorizedPurchaseOrderPayin, preAuthorization, creditedWallet)
        {
            PurchaseOrder = purchaseOrder;
            Debited = purchaseOrder.TotalOnSalePrice;
            Fees = preAuthorization.Order.FeesPrice
        }
        public virtual PurchaseOrder PurchaseOrder { get; private set;}
    }
}