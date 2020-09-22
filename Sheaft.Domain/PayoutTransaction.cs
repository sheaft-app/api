using Sheaft.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Sheaft.Domain.Models
{
    public class PayoutTransaction : Transaction
    {
        private List<TransferTransaction> _transfers;
        protected PayoutTransaction()
        {
        }

        public PayoutTransaction(Guid id, decimal debited, Wallet debitedWallet, BankAccount bankAccount, decimal fees = 0)
            : base(id, TransactionKind.Payout, debitedWallet.User)
        {
            BankAccount = bankAccount;
            Debited = debited;
            Fees = fees;
            DebitedWallet = debitedWallet;
            Reference = "SHEAFT";

            _transfers = new List<TransferTransaction>();
        }

        public virtual Wallet DebitedWallet { get; private set; }
        public virtual BankAccount BankAccount { get; private set; }
        public virtual IReadOnlyCollection<TransferTransaction> Transfers => _transfers?.AsReadOnly();

        public void AddTransfer(TransferTransaction transaction)
        {
            if (Transfers == null)
                _transfers = new List<TransferTransaction>();

            _transfers.Add(transaction);
        }
    }
}