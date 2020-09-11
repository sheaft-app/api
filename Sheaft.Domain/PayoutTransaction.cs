using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Domain.Models
{
    public class PayoutTransaction : Transaction
    {
        protected PayoutTransaction()
        {
        }

        public PayoutTransaction(Guid id, decimal debited, Wallet debitedWallet, BankAccount bankAccount)
            : base(id, TransactionKind.Payout, TransactionNature.Regular, debitedWallet.User)
        {
            BankAccount = bankAccount;
            Debited = debited;
            DebitedWallet = debitedWallet;
            Reference = $"SHEAFT_{DateTimeOffset.UtcNow:yyyyMMdd}";
        }

        public virtual BankAccount BankAccount { get; private set; }
    }
}