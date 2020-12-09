using Sheaft.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sheaft.Domain.Models
{
    public class Payout : Transaction
    {
        private List<Transfer> _transfers;
        private List<Withholding> _withholdings;

        protected Payout()
        {
        }

        public Payout(Guid id, Wallet debitedWallet, BankAccount bankAccount, IEnumerable<Transfer> transfers, IEnumerable<Withholding> withholdings = null)
            : base(id, TransactionKind.Payout, debitedWallet.User)
        {
            BankAccount = bankAccount;
            Debited = transfers.Sum(t => t.Credited) - (withholdings?.Sum(w => w.Debited) ?? 0);
            Fees = 0;
            DebitedWallet = debitedWallet;
            Reference = "SHEAFT";

            _withholdings = withholdings?.ToList();
            _transfers = transfers.ToList();
        }

        public virtual Wallet DebitedWallet { get; private set; }
        public virtual BankAccount BankAccount { get; private set; }
        public virtual IReadOnlyCollection<Transfer> Transfers => _transfers?.AsReadOnly();
        public virtual IReadOnlyCollection<Withholding> Withholdings => _withholdings?.AsReadOnly();
    }
}