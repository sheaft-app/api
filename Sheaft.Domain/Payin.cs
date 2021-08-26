using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Core.Exceptions;
using Sheaft.Domain.Common;
using Sheaft.Domain.Enum;

namespace Sheaft.Domain
{
    public abstract class Payin : Transaction, IHasDomainEvent
    {
        protected Payin()
        {
        }

        protected Payin(Guid id, TransactionKind kind, Wallet creditedWallet, Order order)
            : base(id, kind, creditedWallet.User)
        {
            Order = order;
            OrderId = order.Id;
            Fees = order.FeesPrice;
            Debited = order.TotalPrice;
            CreditedWallet = creditedWallet;
            CreditedWalletId = creditedWallet.Id;
            Reference = "SHEAFT";

            DomainEvents = new List<DomainEvent>();
            Refunds = new List<PayinRefund>();
        }

        public Guid CreditedWalletId { get; private set; }
        public Guid OrderId { get; private set; }
        public int RefundsCount { get; private set; }
        public virtual Wallet CreditedWallet { get; private set; }
        public virtual Order Order { get; private set; }
        public virtual ICollection<PayinRefund> Refunds  { get; private set; }
        
        public void AddRefund(PayinRefund refund)
        {
            if (Refunds != null && Refunds.Any(r => r.PurchaseOrderId == refund.PurchaseOrderId && r.Status == TransactionStatus.Succeeded))
                throw SheaftException.Validation("Impossible d'ajouter un remboursement à ce paiement, il a déjà été remboursé.");

            if (Refunds == null)
                Refunds = new List<PayinRefund>();
            
            Refunds.Add(refund);
            RefundsCount = Refunds?.Count ?? 0;
        }

        public List<DomainEvent> DomainEvents { get; } = new List<DomainEvent>();
    }
}