using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Domain.Models
{
    public class Card : PaymentMethod
    {
        protected Card() { }

        public Card(Guid id, string name)
            : base(id, name, PaymentKind.Card)
        {
        }
    }
}