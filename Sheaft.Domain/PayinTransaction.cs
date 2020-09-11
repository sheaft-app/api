using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Domain.Models
{
    public class PayinTransaction : Transaction
    {
        protected PayinTransaction()
        {
        }

        public PayinTransaction(Guid id, Wallet creditedWallet, Order order)
            : base(id, TransactionKind.Payin, TransactionNature.Regular, creditedWallet.User)
        {
            Order = order;
            Fees = order.Fees;
            Debited = order.TotalWholeSalePrice + order.Fees;
            CreditedWallet = creditedWallet;
            Reference = $"SHEAFT_{DateTimeOffset.UtcNow:yyyyMMdd}";
        }

        public virtual Card Card { get; private set; }
        public virtual Order Order { get; private set; }
    }
}