using Sheaft.Domain.Interop;
using Sheaft.Domain.Enums;
using System;

namespace Sheaft.Domain.Models
{
    public abstract class Legal : IEntity
    {
        protected Legal()
        {
        }

        protected Legal(Guid id, LegalKind kind, User user, Owner owner)
        {
            Id = id;
            Kind = kind;
            Owner = owner;
            User = user;
        }

        public Guid Id { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public DateTimeOffset? RemovedOn { get; private set; }
        public LegalKind Kind { get; protected set; }
        public virtual User User { get; private set; }
        public virtual Owner Owner { get; private set; }
    }
}