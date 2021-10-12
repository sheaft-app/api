using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Core.Exceptions;
using Sheaft.Domain.Enum;

namespace Sheaft.Domain
{
    public abstract class Business : User
    {
        protected Business()
        {
        }

        protected Business(Guid id, ProfileKind kind, string name, string firstname, string lastname, string email, UserAddress address, bool openForBusiness = true, string phone = null)
            : base(id, kind, name, firstname, lastname, email, phone)
        {
            if (address == null)
                throw SheaftException.Validation("L'adresse du siège social est requise.");
            
            SetOpenForNewBusiness(openForBusiness);
            SetAddress(address);

            Closings = new List<BusinessClosing>();
        }

        public bool OpenForNewBusiness { get; private set; }
        public int ClosingsCount { get; private set; }
        public virtual ICollection<BusinessClosing> Closings { get; private set; }

        public void SetName(string name)
        {
            SetUserName(name);
        }

        public void SetOpenForNewBusiness(bool openForNewBusiness)
        {
            OpenForNewBusiness = openForNewBusiness;
        }

        public void AddClosing(BusinessClosing closing)
        {
            if (Closings == null)
                Closings = new List<BusinessClosing>();

            Closings.Add(closing);
            ClosingsCount = Closings?.Count ?? 0;
        }
        
        public void RemoveClosings(IEnumerable<Guid> ids)
        {
            foreach (var id in ids)
                RemoveClosing(id);
        }

        public void RemoveClosing(Guid id)
        {
            var closing = Closings.SingleOrDefault(r => r.Id == id);
            if(closing == null)
                throw SheaftException.NotFound("La plage horaire de fermeture est introuvable.");
            
            Closings.Remove(closing);
            ClosingsCount = Closings?.Count ?? 0;
        }

        public BusinessLegal SetLegals(LegalKind kind, string name, string email, string siret, string vatIdentifier, LegalAddress address, BillingAddress billingAddress, Owner owner, string registrationCity = null, string registrationCode = null, RegistrationKind? registrationKind = null)
        {
            if (Legal?.Id != null)
                throw SheaftException.AlreadyExists("Les informations légales de cette société existent déjà.");

            var legals = new BusinessLegal(Guid.NewGuid(),this, kind, name, email, siret, vatIdentifier, address, owner, billingAddress);
            if(registrationKind.HasValue)
                legals.SetRegistrationKind(registrationKind.Value, registrationCity, registrationCode);
            
            Legal = legals;

            return legals;
        }
    }
}
