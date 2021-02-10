using System;
using Sheaft.Domain.Enum;

namespace Sheaft.Domain
{
    public class BankAccount : PaymentMethod
    {
        protected BankAccount() { }

        public BankAccount(Guid id, string name, string owner, string iban, string bic, BankAddress address, User user)
            : base(id, name, PaymentKind.BankAccount, user)
        {
            IBAN = iban;
            BIC = bic;
            Owner = owner;
            Line1 = address.Line1;
            Line2 = address.Line2;
            Zipcode = address.Zipcode;
            City = address.City;
            Country = address.Country;
        }

        public string IBAN { get; private set; }
        public string BIC { get; private set; }
        public string Owner { get; private set; }
        public string Line1 { get; private set; }
        public string Line2 { get; private set; }
        public string Zipcode { get; private set; }
        public string City { get; private set; }
        public CountryIsoCode Country { get; private set; }

        public void SetAddress(BankAddress address)
        {
            if (address == null)
                return;

            Line1 = address.Line1;
            Line2 = address.Line2;
            Zipcode = address.Zipcode;
            City = address.City;
            Country = address.Country;
        }

        public void SetOwner(string owner)
        {
            if (owner == null)
                return;

            Owner = owner;
        }

        public void SetIban(string iban)
        {
            if (iban == null)
                return;

            IBAN = iban;
        }

        public void SetBic(string bic)
        {
            if (bic == null)
                return;

            BIC = bic;
        }
    }
}