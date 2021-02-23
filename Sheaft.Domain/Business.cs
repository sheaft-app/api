using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Domain.Common;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Exceptions;

namespace Sheaft.Domain
{
    public abstract class Business : User
    {
        private List<BusinessClosing> _closings;
        protected Business()
        {
        }

        protected Business(Guid id, ProfileKind kind, string name, string firstname, string lastname, string email, UserAddress address, bool openForBusiness = true, string phone = null, string description = null)
            : base(id, kind, name, firstname, lastname, email, phone)
        {
            if (address == null)
                throw new ValidationException(MessageKind.User_Address_Required);
            
            SetOpenForNewBusiness(openForBusiness);
            SetDescription(description);

            SetAddress(address);
        }

        public bool OpenForNewBusiness { get; private set; }
        public string Description { get; private set; }
        public virtual IReadOnlyCollection<BusinessClosing> Closings => _closings?.AsReadOnly(); 

        public void SetName(string name)
        {
            SetUserName(name);
        }

        public void SetOpenForNewBusiness(bool openForNewBusiness)
        {
            OpenForNewBusiness = openForNewBusiness;
        }

        public void SetDescription(string description)
        {
            if (description == null)
                return;

            Description = description;
        }

        public BusinessClosing AddClosing(DateTimeOffset from, DateTimeOffset to, string reason = null)
        {
            if (Closings == null)
                _closings = new List<BusinessClosing>();

            var closing = new BusinessClosing(Guid.NewGuid(), from, to, reason);
            _closings.Add(closing);

            return closing;
        }

        public void RemoveClosing(Guid id)
        {
            var closing = _closings.SingleOrDefault(r => r.Id == id);
            if(closing == null)
                throw SheaftException.NotFound();
            
            _closings.Remove(closing);
        }
    }
}