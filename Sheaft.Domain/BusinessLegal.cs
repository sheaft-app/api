using System;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Core.Extensions;
using Sheaft.Domain.Enum;

namespace Sheaft.Domain
{
    public class BusinessLegal : Legal
    {
        protected BusinessLegal()
        {
        }

        public BusinessLegal(Guid id, Business business, LegalKind kind, string name, string email, string siret, string vatIdentifier, LegalAddress address, Owner owner)
            : base(id, kind, business, owner)
        {
            SetName(name);
            SetEmail(email);
            SetAddress(address);
            SetSiret(siret);
            SetVatIdentifier(vatIdentifier);
        }

        public string Name { get; set; }
        public string Email { get; private set; }
        public string Siret { get; private set; }
        public string VatIdentifier { get; private set; }
        public virtual LegalAddress Address { get; private set; }
        public virtual Declaration Declaration { get; private set; }

        public void SetDeclaration()
        {
            if (Declaration != null)
                Declaration = null;

            Declaration = new Declaration(Guid.NewGuid());
        }

        public void SetSiret(string siret)
        {
            if (siret.IsNotNullAndIsEmptyOrWhiteSpace())
                throw new ValidationException(MessageKind.Legal_Siret_Required);

            Siret = siret;
        }

        public void SetVatIdentifier(string vatNumber)
        {
            if (vatNumber.IsNotNullAndIsEmptyOrWhiteSpace())
                throw new ValidationException(MessageKind.Legal_Vat_Required);

            VatIdentifier = vatNumber;
        }

        public override void SetKind(LegalKind kind)
        {
            if (kind == LegalKind.Natural)
                throw new ValidationException(MessageKind.Legal_Kind_Cannot_Be_Natural);

            base.SetKind(kind);
        }

        public void SetAddress(LegalAddress legalAddress)
        {
            Address = legalAddress;
        }

        public void SetEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ValidationException(MessageKind.Legal_Email_Required);

            Email = email;
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ValidationException(MessageKind.Legal_Name_Required);

            Name = name;
        }
    }
}