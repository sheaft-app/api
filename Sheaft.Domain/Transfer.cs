using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Domain.Models
{
    public class Transfer : PaymentMethod
    {
        protected Transfer() { }

        public Transfer(Guid id, string name, string owner, string iban, string bic, Address address)
            : base(id, name, PaymentKind.Transfer)
        {
            IBAN = iban;
            BIC = bic;
            OwnerName = owner;
            OwnerAddress = address;
        }

        public string IBAN { get; private set; }
        public string BIC { get; private set; }
        public string OwnerName { get; private set; }
        public virtual Address OwnerAddress { get; private set; }

    }
}