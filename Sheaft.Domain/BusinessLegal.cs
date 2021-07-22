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

        public string Name { get; private set; }
        public string Identifier { get; private set; }
        public string Email { get; private set; }
        public string Siret { get; private set; }
        public string VatIdentifier { get; private set; }
        public Guid? DeclarationId { get; private set; }
        public LegalAddress Address { get; private set; }
        public virtual Declaration Declaration { get; private set; }

        public void SetDeclaration()
        {
            Declaration = new Declaration(Guid.NewGuid());
            DeclarationId = Declaration.Id;
        }

        public void SetSiret(string siret)
        {
            if (siret.IsNotNullAndIsEmptyOrWhiteSpace())
                throw SheaftException.Validation("Le numéro de SIRET est requis.");

            Siret = siret.Trim();
        }

        public void SetVatIdentifier(string vatNumber)
        {
            if (vatNumber.IsNotNullAndIsEmptyOrWhiteSpace())
                throw SheaftException.Validation("Le numéro de TVA est requis.");

            VatIdentifier = vatNumber;
        }

        public override void SetKind(LegalKind kind)
        {
            if (kind == LegalKind.Natural)
                throw SheaftException.Validation("Une statut légal d'une société ne peut pas être de type personnel.");

            base.SetKind(kind);
        }

        public void SetAddress(LegalAddress legalAddress)
        {
            Address = legalAddress;
        }

        public void SetEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw SheaftException.Validation("L'email de contact de l'entreprise est requis.");

            Email = email;
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw SheaftException.Validation("Le nom de l'entreprise est requis.");

            Name = name;
        }

        public void SetRegistrationKind(RegistrationKind kind, string city, string code)
        {
            if(kind == RegistrationKind.RCS)
                Identifier = $"{kind:G} {city.Trim()} {Siret.Substring(0, 9)} ";
            
            if(kind == RegistrationKind.RM)
                Identifier = $"{kind:G} {Siret.Substring(0, 9)} {code.Trim()}";
        }
    }
}