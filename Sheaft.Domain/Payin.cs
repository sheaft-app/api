using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain.Common;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events.Payin;

namespace Sheaft.Domain
{
    public abstract class Payin : Transaction, IHasDomainEvent
    {
        private List<PayinRefund> _refunds;

        protected Payin()
        {
        }

        protected Payin(Guid id, TransactionKind kind, Wallet creditedWallet, Order order)
            : base(id, kind, creditedWallet.User)
        {
            Order = order;
            Fees = order.FeesPrice;
            Debited = order.TotalPrice;
            CreditedWallet = creditedWallet;
            Reference = "SHEAFT";

            DomainEvents = new List<DomainEvent>();
            _refunds = new List<PayinRefund>();
        }

        public virtual Wallet CreditedWallet { get; private set; }
        public virtual Order Order { get; private set; }
        public virtual IReadOnlyCollection<PayinRefund> Refunds => _refunds.AsReadOnly();

        public void AddRefund(PayinRefund refund)
        {
            if (Refunds != null && Refunds.Any(r => r.PurchaseOrder.Id == refund.PurchaseOrder.Id && r.Status == TransactionStatus.Succeeded))
                throw new ValidationException(MessageKind.Payin_CannotAdd_Refund_PurchaseOrderRefund_AlreadySucceeded);

            _refunds.Add(refund);
        }

        public override void SetStatus(TransactionStatus status)
        {
            base.SetStatus(status);
            
            switch (Status)
            {
                case TransactionStatus.Failed:
                    DomainEvents.Add(new PayinFailedEvent(Id));
                    break;
                case TransactionStatus.Succeeded:
                    DomainEvents.Add(new PayinSucceededEvent(Id));
                    break;
            }
        }

        public List<DomainEvent> DomainEvents { get; } = new List<DomainEvent>();
    }
}