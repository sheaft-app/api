using System;
using System.Collections.Generic;
using Sheaft.Domain.Common;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events.PayinRefund;

namespace Sheaft.Domain
{
    public class PayinRefund : Refund, IHasDomainEvent
    {
        protected PayinRefund()
        {
        }

        public PayinRefund(Guid id, Payin transaction, PurchaseOrder purchaseOrder)
            : base(id, TransactionKind.RefundPayin, purchaseOrder.TotalOnSalePrice, transaction.CreditedWallet, transaction.Author)
        {
            Credited = transaction.Debited;
            PurchaseOrder = purchaseOrder;
            PurchaseOrderId = purchaseOrder.Id;
            Payin = transaction;
            PayinId = transaction.Id;
            DomainEvents = new List<DomainEvent>();
        }

        public Guid PayinId { get; private set; }
        public Guid PurchaseOrderId { get; private set; }
        public virtual Payin Payin { get; private set; }
        public virtual PurchaseOrder PurchaseOrder { get; private set; }

        public override void SetStatus(TransactionStatus status)
        {
            base.SetStatus(status);
            
            switch (Status)
            {
                case TransactionStatus.Failed:
                    DomainEvents.Add(new PayinRefundFailedEvent(Id));
                    break;
            }
        }

        public List<DomainEvent> DomainEvents { get; } = new List<DomainEvent>();
    }
}