using Sheaft.Domain.Enums;
using Sheaft.Domains.Extensions;
using Sheaft.Exceptions;
using System;
using System.Collections.Generic;

namespace Sheaft.Domain.Models
{
    public class BusinessLegal : Legal
    {
        protected BusinessLegal()
        {
        }

        public BusinessLegal(Guid id, Business business, LegalKind kind, string email, string siret, string vatIdentifier, LegalAddress address, Owner owner)
            : base(id, kind, business, owner)
        {
            SetEmail(email);
            SetAddress(address);
            SetSiret(siret);
            SetVatIdentifier(vatIdentifier);
        }

        public string Email { get; private set; }
        public string Siret { get; private set; }
        public string VatIdentifier { get; private set; }
        public virtual LegalAddress Address { get; private set; }
        public virtual UboDeclaration UboDeclaration { get; private set; }

        public void SetUboDeclaration(UboDeclaration declaration)
        {
            if (UboDeclaration != null)
                UboDeclaration = null;

            UboDeclaration = declaration;
        }

        public void SetSiret(string siret)
        {
            if (siret.IsNotNullAndIsEmptyOrWhiteSpace())
                throw new ValidationException(MessageKind.Business_Siret_Required);

            Siret = siret;
        }

        public void SetVatIdentifier(string vatNumber)
        {
            if (vatNumber.IsNotNullAndIsEmptyOrWhiteSpace())
                throw new ValidationException(MessageKind.Business_Vat_Required);

            VatIdentifier = vatNumber;
        }

        public void SetKind(LegalKind kind)
        {
            if (kind == LegalKind.Natural)
                throw new ValidationException();

            Kind = kind;
        }

        public void SetAddress(LegalAddress legalAddress)
        {
            Address = legalAddress;
        }

        public void SetEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ValidationException();

            Email = email;
        }
    }
}