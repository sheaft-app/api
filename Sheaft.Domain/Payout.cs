﻿using Sheaft.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Sheaft.Domain.Models
{
    public class Payout : Transaction
    {
        private List<Transfer> _transfers;
        protected Payout()
        {
        }

        public Payout(Guid id, decimal debited, Wallet debitedWallet, BankAccount bankAccount, decimal fees = 0)
            : base(id, TransactionKind.Payout, debitedWallet.User)
        {
            BankAccount = bankAccount;
            Debited = debited;
            Fees = fees;
            DebitedWallet = debitedWallet;
            Reference = "SHEAFT";

            _transfers = new List<Transfer>();
        }

        public virtual Wallet DebitedWallet { get; private set; }
        public virtual BankAccount BankAccount { get; private set; }
        public virtual IReadOnlyCollection<Transfer> Transfers => _transfers?.AsReadOnly();

        public void AddTransfer(Transfer transaction)
        {
            if (Transfers == null)
                _transfers = new List<Transfer>();

            _transfers.Add(transaction);
        }
    }
}