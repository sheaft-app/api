using System;
using System.Collections.Generic;
using Sheaft.Domain.Common;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events.Donation;

namespace Sheaft.Domain
{
    public class Donation : Transaction, IHasDomainEvent
    {
        protected Donation()
        {
        }

        public Donation(Guid id, Wallet debitedWallet, Wallet creditedWallet, Order order)
            : base(id, TransactionKind.Donation, debitedWallet.User)
        {
            Order = order;
            Fees = 0;
            Debited = Math.Round(order.Donation - order.DonationFeesPrice, 2);
            CreditedWallet = creditedWallet;
            DebitedWallet = debitedWallet;
            Credited = Debited;
            Reference = "DONATION";
            DomainEvents = new List<DomainEvent>();
        }

        public virtual Wallet CreditedWallet { get; private set; }
        public virtual Wallet DebitedWallet { get; private set; }
        public virtual Order Order { get; private set; }
        

        public override void SetStatus(TransactionStatus status)
        {
            base.SetStatus(status);
            
            switch (Status)
            {
                case TransactionStatus.Failed:
                    DomainEvents.Add(new DonationFailedEvent(Id));
                    break;
            }
        }

        public List<DomainEvent> DomainEvents { get; } = new List<DomainEvent>();
    }
}