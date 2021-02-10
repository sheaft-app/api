using System;
using Sheaft.Domain.Enum;

namespace Sheaft.Domain
{
    public class CardPayin : Payin
    {
        protected CardPayin()
        {
        }

        public CardPayin(Guid id, Wallet creditedWallet, Card card, Order order)
            : base(id, TransactionKind.PayinCard, creditedWallet, order)
        {
            Card = card;
        }

        public virtual Card Card { get; }
    }
}