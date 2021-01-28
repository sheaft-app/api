using Sheaft.Domain.Enums;
using System;

namespace Sheaft.Domain.Models
{
    public class WebPayinRefund : Refund
    {
        protected WebPayinRefund()
        {
        }

        public WebPayinRefund(Guid id, WebPayin transaction, PurchaseOrder purchaseOrder)
            : base(id, TransactionKind.RefundPayin, purchaseOrder.TotalOnSalePrice, transaction.CreditedWallet, transaction.Author)
        {
            Credited = transaction.Debited;
            Payin = transaction;
            PurchaseOrder = purchaseOrder;
        }

        public virtual WebPayin Payin { get; private set; }
        public virtual PurchaseOrder PurchaseOrder { get; private set; }
    }
}