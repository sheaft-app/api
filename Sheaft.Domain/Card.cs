using System;
using Sheaft.Domain.Enum;

namespace Sheaft.Domain
{
    public class Card : PaymentMethod
    {
        protected Card() { }

        public Card(Guid id, string name, User user)
            : base(id, name, PaymentKind.Card, user)
        {
        }
    }
}