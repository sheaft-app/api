using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Domain.Models
{
    public class CardPayinTransaction : PayinTransaction
    {
        protected CardPayinTransaction()
        {
        }

        public CardPayinTransaction(Guid id, Wallet creditedWallet, Card card, Order order)
            : base(id, TransactionKind.PayinCard, creditedWallet, order)
        {
            Card = card;
        }

        public virtual Card Card { get; private set; }
    }
}