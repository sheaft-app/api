using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Domain.Common;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events.Payout;

namespace Sheaft.Domain
{
    public class Payout : Transaction, IHasDomainEvent
    {
        protected Payout()
        {
        }

        public Payout(Guid id, Wallet debitedWallet, BankAccount bankAccount, IEnumerable<Transfer> transfers,
            IEnumerable<Withholding> withholdings)
            : base(id, TransactionKind.Payout, debitedWallet.User)
        {
            BankAccount = bankAccount;
            BankAccountId = bankAccount.Id;
            Debited = transfers.Sum(t => t.Credited) - withholdings.Sum(w => w.Debited);
            Fees = 0;
            DebitedWallet = debitedWallet;
            DebitedWalletId = debitedWallet.Id;
            Reference = "SHEAFT";

            DomainEvents = new List<DomainEvent>();
            Withholdings = withholdings.ToList();
            WithholdingsCount = Withholdings?.Count ?? 0;
            
            Transfers = transfers.ToList();
            TransfersCount = Transfers?.Count ?? 0;
        }

        public Guid BankAccountId { get; private set; }
        public Guid DebitedWalletId { get; private set; }
        public int TransfersCount { get; private set; }
        public int WithholdingsCount { get; private set; }
        public virtual Wallet DebitedWallet { get; private set; }
        public virtual BankAccount BankAccount { get; private set; }
        public virtual ICollection<Transfer> Transfers { get; private set; }
        public virtual ICollection<Withholding> Withholdings { get; private set; }

        public override void SetStatus(TransactionStatus status)
        {
            base.SetStatus(status);

            switch (Status)
            {
                case TransactionStatus.Failed:
                    DomainEvents.Add(new PayoutFailedEvent(Id));
                    break;
            }
        }

        public List<DomainEvent> DomainEvents { get; } = new List<DomainEvent>();
    }
}