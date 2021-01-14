using Sheaft.Domain.Enums;
using System;

namespace Sheaft.Domain.Models
{
    public class CardPayin : Payin
    {
        protected CardPayin()
        {
        }

        public CardPayin(Guid id, Wallet creditedWallet, Card card, Order order)
            : base(id, TransactionKind.CardPayin, creditedWallet, order)
        {
            Card = card;
        }

        public virtual Card Card { get; }
    }
}