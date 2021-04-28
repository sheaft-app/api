using System;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public abstract class PaymentMethod : IEntity
    {
        protected PaymentMethod()
        {
        }

        protected PaymentMethod(Guid id, string name, PaymentKind kind, User user)
        {
            Id = id;
            Name = name;
            Kind = kind;
            IsActive = true;
            User = user;
            UserId = user.Id;
        }

        public Guid Id { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public DateTimeOffset? RemovedOn { get; private set; }
        public string Identifier { get; private set; }
        public string Name { get; private set; }
        public PaymentKind Kind { get; private set; }
        public bool IsActive { get; private set; }
        public Guid UserId { get; private set; }
        public virtual User User { get; private set; }

        public void SetIdentifier(string identifier)
        {
            if (identifier == null)
                return;

            Identifier = identifier;
        }

        public void SetIsActive(bool isActive)
        {
            IsActive = isActive;
        }

        public void SetName(string name)
        {
            if (name == null)
                return;

            Name = name;
        }
    }
}