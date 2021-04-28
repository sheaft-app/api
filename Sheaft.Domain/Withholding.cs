using System;
using System.Collections.Generic;
using Sheaft.Domain.Common;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events.Withholding;

namespace Sheaft.Domain
{
    public class Withholding : Transaction, IHasDomainEvent
    {
        protected Withholding()
        {
        }

        public Withholding(Guid id, decimal amount, Wallet debitedWallet, Wallet creditedWallet)
            : base(id, TransactionKind.Withholding, debitedWallet.User)
        {
            Fees = 0;
            Debited = amount;
            CreditedWallet = creditedWallet;
            CreditedWalletId = creditedWallet.Id;
            DebitedWallet = debitedWallet;
            DebitedWalletId = debitedWallet.Id;
            Credited = Debited;
            Reference = "WITHHOLDING";
            DomainEvents = new List<DomainEvent>();
        }
        
        public Guid CreditedWalletId { get; private set; }
        public Guid DebitedWalletId { get; private set; }
        public Guid? PayoutId { get; private set; }
        public virtual Wallet CreditedWallet { get; private set; }
        public virtual Wallet DebitedWallet { get; private set; }
        public virtual Payout Payout { get; private set; }

        public override void SetStatus(TransactionStatus status)
        {
            base.SetStatus(status);
            
            switch (Status)
            {
                case TransactionStatus.Failed:
                    DomainEvents.Add(new WithholdingFailedEvent(Id));
                    break;
            }
        }

        public List<DomainEvent> DomainEvents { get; } = new List<DomainEvent>();
    }
}