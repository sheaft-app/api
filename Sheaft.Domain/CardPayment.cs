using Sheaft.Exceptions;
using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Domain.Models
{

    public class CardPayment : PaymentMethod
    {
        protected CardPayment() { }

        public CardPayment(Guid id, string identifier, string name, string owner, DateTimeOffset validity)
            : base(id, identifier, name, owner, PaymentKind.Card)
        {
            Expired = false;
            Validity = validity;
        }

        public bool Expired { get; private set; }
        public DateTimeOffset Validity { get; private set; }
    }
}