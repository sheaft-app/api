using Sheaft.Domains.Extensions;
using Sheaft.Exceptions;
using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Domain.Models
{
    public abstract class Company : User
    {
        protected Company()
        {
        }

        protected Company(Guid id, ProfileKind kind, LegalKind legal, string name, string email, string firstname, string lastname, string siret, string vatIdentifier, Address address, bool openForBusiness = true, string phone = null, string description = null)
            : base(id, kind, legal, name, firstname, lastname, email, phone)
        {
            if (address == null)
                throw new ValidationException(MessageKind.Company_Address_Required);

            SetOpenForNewBusiness(openForBusiness);
            SetSiret(siret);
            SetVatIdentifier(vatIdentifier);
            SetDescription(description);
            SetAddress(address.Line1, address.Line2, address.Zipcode, address.City, address.Country, address.Department, address.Longitude, address.Latitude);
        }

        public bool OpenForNewBusiness { get; private set; }
        public string Description { get; private set; }
        public string VatIdentifier { get; private set; }
        public string Siret { get; private set; }
        public virtual LegalAddress LegalAddress { get; private set; }

        public void SetName(string name)
        {
            SetUserName(name);
        }

        public void SetLegalAddress(string line1, string line2, string zipcode, string city, string country)
        {
            LegalAddress = new LegalAddress(line1, line2, zipcode, city, country);
        }

        public void SetOpenForNewBusiness(bool openForNewBusiness)
        {
            OpenForNewBusiness = openForNewBusiness;
        }

        public void SetSiret(string siret)
        {
            if (siret.IsNotNullAndIsEmptyOrWhiteSpace())
                throw new ValidationException(MessageKind.Company_Siret_Required);

            Siret = siret;
        }

        public void SetDescription(string description)
        {
            if (description == null)
                return;

            Description = description;
        }

        public void SetVatIdentifier(string vatNumber)
        {
            if (vatNumber.IsNotNullAndIsEmptyOrWhiteSpace())
                throw new ValidationException(MessageKind.Company_Vat_Required);

            VatIdentifier = vatNumber;
        }

        public override void Close(string reason)
        {
            base.Close(reason);

            SetAddress("", "", Address.Zipcode, "", null, null);
        }
    }
}