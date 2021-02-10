using Sheaft.Domain.Enums;
using System;
using Sheaft.Domains.Exceptions;

namespace Sheaft.Domain.Models
{
    public abstract class Business : User
    {
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