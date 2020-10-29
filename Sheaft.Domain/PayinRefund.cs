using Sheaft.Domain.Enums;
using System;

namespace Sheaft.Domain.Models
{
    public class PayinRefund : Refund
    {
        protected PayinRefund()
        {
        }

        public PayinRefund(Guid id, Payin transaction, PurchaseOrder purchaseOrder)
            : base(id, TransactionKind.RefundPayin, purchaseOrder.TotalOnSalePrice, transaction.CreditedWallet, transaction.Author)
        {
            Credited = transaction.Debited;
            Payin = transaction;
            PurchaseOrder = purchaseOrder;
        }

        public virtual Payin Payin { get; private set; }
        public virtual PurchaseOrder PurchaseOrder { get; private set; }
    }
}