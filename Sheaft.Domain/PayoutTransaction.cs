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
            : base(id, TransactionKind.Payout, debitedWallet.User)
        {
            BankAccount = bankAccount;
            Debited = debited;
            DebitedWallet = debitedWallet;
            Reference = $"SHEAFT_{DateTimeOffset.UtcNow:yyyyMMdd}";
        }

        public virtual Wallet DebitedWallet { get; private set; }
        public virtual BankAccount BankAccount { get; private set; }
    }
}