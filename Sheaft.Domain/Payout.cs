using Sheaft.Domain.Enums;
using Sheaft.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sheaft.Domain.Models
{
    public class Payout : Transaction
    {
        private List<Transfer> _transfers;
        protected Payout()
        {
        }

        public Payout(Guid id, decimal debited, Wallet debitedWallet, BankAccount bankAccount, IEnumerable<Transfer> transfers, decimal fees = 0)
            : base(id, TransactionKind.Payout, debitedWallet.User)
        {
            BankAccount = bankAccount;
            Debited = debited;
            Fees = fees;
            DebitedWallet = debitedWallet;
            Reference = "SHEAFT";

            _transfers = transfers.ToList();
        }

        public virtual Wallet DebitedWallet { get; private set; }
        public virtual BankAccount BankAccount { get; private set; }
        public virtual IReadOnlyCollection<Transfer> Transfers => _transfers?.AsReadOnly();
    }
}