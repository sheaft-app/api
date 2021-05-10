using System;
using System.Collections.Generic;
using Sheaft.Domain.Common;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events.Transfer;

namespace Sheaft.Domain
{
    public class Transfer : Transaction, IHasDomainEvent
    {
        protected Transfer()
        {
        }

        public Transfer(Guid id, Wallet debitedWallet, Wallet creditedWallet, PurchaseOrder purchaseOrder)
            : base(id, TransactionKind.Transfer, debitedWallet.User)
        {
            PurchaseOrder = purchaseOrder;
            PurchaseOrderId = purchaseOrder.Id;
            Debited = purchaseOrder.TotalWholeSalePrice;
            Credited = purchaseOrder.TotalWholeSalePrice;
            CreditedWallet = creditedWallet;
            CreditedWalletId = creditedWallet.Id;
            DebitedWallet = debitedWallet;
            DebitedWalletId = debitedWallet.Id;
            Reference = "SHEAFT";
            
            DomainEvents = new List<DomainEvent>();
        }
        
        public Guid PurchaseOrderId { get; private set; }
        public Guid CreditedWalletId { get; private set; }
        public Guid DebitedWalletId { get; private set; }
        public Guid? PayoutId { get; private set; }
        public virtual PurchaseOrder PurchaseOrder { get; private set; }
        public virtual Wallet CreditedWallet { get; private set; }
        public virtual Wallet DebitedWallet { get; private set; }
        public virtual Payout Payout { get; private set; }

        public override void SetStatus(TransactionStatus status)
        {
            base.SetStatus(status);
            
            switch (Status)
            {
                case TransactionStatus.Failed:
                    DomainEvents.Add(new TransferFailedEvent(Id));
                    break;
            }
        }

        public List<DomainEvent> DomainEvents { get; } = new List<DomainEvent>();
    }
}