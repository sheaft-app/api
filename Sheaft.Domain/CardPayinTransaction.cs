using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Domain.Models
{
    public class CardPayinTransaction : Transaction
    {
        protected CardPayinTransaction()
        {
        }

        public CardPayinTransaction(Guid id, Wallet creditedWallet, Card card, Order order)
            : base(id, TransactionKind.PayinCard, creditedWallet.User)
        {
            Card = card;
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