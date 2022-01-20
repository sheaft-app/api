using System;

namespace Sheaft.Domain
{
    public class CompanyBilling
    {
        protected CompanyBilling()
        {
        }

        internal CompanyBilling(Guid companyId, string iban, string bic, BillingAddress address)
        {
            CompanyId = companyId;
            Address = address;
            IBAN = iban;
            BIC = bic;
        }

        public Guid CompanyId { get; set; }
        public DateTimeOffset UpdatedOn { get; private set; }
        public BillingAddress Address { get; protected set; }
        public string IBAN { get; private set; }
        public string BIC { get; private set; }
        public string PaymentExigibleIn { get; set; }
    }
}