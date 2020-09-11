using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Domain.Models
{
    public class BankAccount : PaymentMethod
    {
        protected BankAccount() { }

        public BankAccount(Guid id, string name, string owner, string iban, string bic, BankAddress address, User user)
            : base(id, name, PaymentKind.BankAccount, user)
        {
            IBAN = iban;
            BIC = bic;
            OwnerName = owner;
            OwnerAddress = address;
        }

        public string IBAN { get; private set; }
        public string BIC { get; private set; }
        public string OwnerName { get; private set; }
        public virtual BankAddress OwnerAddress { get; private set; }

    }
}