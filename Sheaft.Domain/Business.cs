using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain.Common;
using Sheaft.Domain.Enum;

namespace Sheaft.Domain
{
    public abstract class Business : User
    {
        private List<BusinessClosing> _closings;
        protected Business()
        {
        }

        protected Business(Guid id, ProfileKind kind, string name, string firstname, string lastname, string email, UserAddress address, bool openForBusiness = true, string phone = null)
            : base(id, kind, name, firstname, lastname, email, phone)
        {
            if (address == null)
                throw new ValidationException(MessageKind.User_Address_Required);
            
            SetOpenForNewBusiness(openForBusiness);
            SetAddress(address);
        }

        public bool OpenForNewBusiness { get; private set; }
        public virtual IReadOnlyCollection<BusinessClosing> Closings => _closings?.AsReadOnly(); 

        public void SetName(string name)
        {
            SetUserName(name);
        }

        public void SetOpenForNewBusiness(bool openForNewBusiness)
        {
            OpenForNewBusiness = openForNewBusiness;
        }

        public BusinessClosing AddClosing(DateTimeOffset from, DateTimeOffset to, string reason = null)
        {
            if (Closings == null)
                _closings = new List<BusinessClosing>();

            var closing = new BusinessClosing(Guid.NewGuid(), from, to, reason);
            _closings.Add(closing);

            return closing;
        }
        
        public void RemoveClosings(IEnumerable<Guid> ids)
        {
            foreach (var id in ids)
                RemoveClosing(id);
        }

        public void RemoveClosing(Guid id)
        {
            var closing = _closings.SingleOrDefault(r => r.Id == id);
            if(closing == null)
                throw SheaftException.NotFound();
            
            _closings.Remove(closing);
        }

        public BusinessLegal SetLegals(LegalKind kind, string name, string email, string siret, string vatIdentifier, LegalAddress address, Owner owner)
        {
            if (Legal?.Id != null)
                throw SheaftException.AlreadyExists();

            var legals = new BusinessLegal(Guid.NewGuid(),this, kind, name, email, siret, vatIdentifier, address, owner);
            Legal = legals;

            return legals;
        }
    }
}
