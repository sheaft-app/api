using Sheaft.Exceptions;
using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Domain.Models
{

    public class TransferPayment : PaymentMethod
    {
        protected TransferPayment() { }

        public TransferPayment(Guid id, string identifier, string name, string owner, string iban, string bic)
            : base(id, identifier, name, owner, PaymentKind.Transfer)
        {
            IBAN = iban;
            BIC = bic;
        }

        public string IBAN { get; private set; }
        public string BIC { get; private set; }
    }
}