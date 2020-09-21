using Sheaft.Domain.Enums;
using Sheaft.Exceptions;
using System;

namespace Sheaft.Domain.Models
{
    public class BusinessLegal : Legal
    {
        
        protected BusinessLegal()
        {
        }

        public BusinessLegal(Guid id, Business business, LegalKind kind, string email, LegalAddress address, Owner owner)
            : base(id, kind, owner)
        {
            SetEmail(email);
            SetAddress(address);

            Business = business;
        }

        public string Email { get; private set; }
        public virtual LegalAddress Address { get; private set; }
        public virtual Business Business { get; private set; }
        public virtual UboDeclaration UboDeclaration { get; private set; }

        public void SetUboDeclaration(UboDeclaration declaration)
        {
            if (UboDeclaration != null)
                UboDeclaration = null;

            UboDeclaration = declaration;
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