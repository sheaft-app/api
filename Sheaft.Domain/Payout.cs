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
        private List<Transfer> _transfers;
        private List<Withholding> _withholdings;

        protected Payout()
        {
        }

        public Payout(Guid id, Wallet debitedWallet, BankAccount bankAccount, IEnumerable<Transfer> transfers,
            IEnumerable<Withholding> withholdings)
            : base(id, TransactionKind.Payout, debitedWallet.User)
        {
            BankAccount = bankAccount;
            Debited = transfers.Sum(t => t.Credited) - withholdings.Sum(w => w.Debited);
            Fees = 0;
            DebitedWallet = debitedWallet;
            Reference = "SHEAFT";

            DomainEvents = new List<DomainEvent>();
            _withholdings = withholdings.ToList();
            _transfers = transfers.ToList();
        }

        public virtual Wallet DebitedWallet { get; private set; }
        public virtual BankAccount BankAccount { get; private set; }
        public virtual IReadOnlyCollection<Transfer> Transfers => _transfers?.AsReadOnly();
        public virtual IReadOnlyCollection<Withholding> Withholdings => _withholdings?.AsReadOnly();

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