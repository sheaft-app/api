using Sheaft.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Domains.Exceptions;

namespace Sheaft.Domain.Models
{
    public abstract class Payin : Transaction
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
    }
}