using Sheaft.Domain.Enums;
using System;

namespace Sheaft.Domain.Models
{
    public class Card : PaymentMethod
    {
        protected Card() { }

        public Card(Guid id, string identifier, string name, bool? remember, CardRegistration cardRegistration)
            : base(id, name ?? $"Carte_{DateTime.UtcNow.ToString("yyyyMMddTHHmmss")}", PaymentKind.Card, cardRegistration.User)
        {
            Remember = remember ?? false;
            CardRegistration = cardRegistration;
        }

        public string ExpirationDate { get; set; }
        public string CardNumber { get; set; }
        public string Provider { get; set; }
        public string BankCode { get; set; }
        public CountryIsoCode Country { get; set; }
        public CardValidity Validity { get; set; }
        public string Fingerprint { get; set; }
        public bool Remember { get; set; }
        public virtual CardRegistration CardRegistration { get; set; }
    }
}