using Sheaft.Domains.Extensions;
using Sheaft.Exceptions;
using Sheaft.Domain.Enums;
using System;

namespace Sheaft.Domain.Models
{
    public abstract class Business : User
    {
        protected Business()
        {
        }

        protected Business(Guid id, ProfileKind kind, string name, string email, string firstname, string lastname, UserAddress address, bool openForBusiness = true, string phone = null, string description = null)
            : base(id, kind, name, firstname, lastname, email, phone)
        {
            if (address == null)
                throw new ValidationException(MessageKind.Business_Address_Required);

            SetOpenForNewBusiness(openForBusiness);
            SetDescription(description);

            SetAddress(address);            
        }

        public bool OpenForNewBusiness { get; private set; }
        public string Description { get; private set; }

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
    }
}