using System;
using Sheaft.Core.Exceptions;
using Sheaft.Domain.Enum;

namespace Sheaft.Domain
{
    public class Consumer : User
    {
        protected Consumer()
        {
        }

        public Consumer(Guid id, string email, string firstname, string lastname, string phone = null)
            : base(id, ProfileKind.Consumer, $"{firstname} {lastname}", firstname, lastname, email, phone)
        {
            Anonymous = true;
        }

        public bool Anonymous { get; private set; }

        public void SetAnonymous(bool anonymous)
        {
            Anonymous = anonymous;
        }

        public ConsumerLegal SetLegals(Owner owner)
        {
            if (LegalId.HasValue)
                throw SheaftException.AlreadyExists();

            var legals = new ConsumerLegal(Guid.NewGuid(),this, owner);
            Legal = legals;
            LegalId = legals.Id;

            return legals;
        }
    }
}