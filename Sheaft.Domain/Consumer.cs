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
            if (Legal?.Id != null)
                throw SheaftException.AlreadyExists("Les informations légales de cet utilisateur existent déjà.");

            var legals = new ConsumerLegal(Guid.NewGuid(),this, owner);
            Legal = legals;

            return legals;
        }
    }
}